-- tao cau hoi
CREATE OR ALTER PROCEDURE SP_HoiDap_TaoCauHoi
    @MaNguoiGui INT,
    @NoiDung NVARCHAR(4000),
    @MaNguoiNhan NVARCHAR(MAX) -- JSON dạng [1,2,3]
AS
BEGIN
BEGIN TRY
    BEGIN TRANSACTION;
    SET NOCOUNT ON;

    -- Khai báo bảng biến để nhận giá trị OUTPUT
    DECLARE @Inserted TABLE (MaTroChuyen UNIQUEIDENTIFIER);

    -- Thêm câu hỏi
    INSERT INTO LX_HoiDap (MaLuongTroChuyen, MaNguoiGui, NgayGui, NoiDung)
    OUTPUT inserted.MaTroChuyen INTO @Inserted
    VALUES (NULL, @MaNguoiGui, GETDATE(), @NoiDung);

    DECLARE @MaTroChuyen UNIQUEIDENTIFIER;
    SELECT TOP 1 @MaTroChuyen = MaTroChuyen FROM @Inserted;

    -- Parse JSON người nhận
    INSERT INTO LX_HoiDap_NguoiNhan (MaTroChuyen, MaNguoiNhan)
    SELECT @MaTroChuyen, CAST([value] AS INT)
    FROM OPENJSON(@MaNguoiNhan);

    COMMIT TRANSACTION;

    -- Gọi SP_HoiDap_ChiTietHoiDap để trả toàn bộ thông tin
    -- tăng hiệu xuất nhưng lặp lại việc đọc và trả kết quả trong repository ở TaoCauHoi vs ChiTietHoiDap
    EXEC SP_HoiDap_ChiTietHoiDap @MaTroChuyen = @MaTroChuyen, @MaNguoiDung = @MaNguoiGui;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH
END


-- Chi tiet hoi dap
go
CREATE OR ALTER PROCEDURE SP_HoiDap_ChiTietHoiDap
    @MaTroChuyen UNIQUEIDENTIFIER,
    @MaNguoiDung int
AS
BEGIN
    SET NOCOUNT ON;

    -- Cập nhật NgayNhan nếu chưa nhận
    UPDATE LX_HoiDap_NguoiNhan
    SET NgayNhan = GETDATE()
    WHERE MaNguoiNhan = @MaNguoiDung
      AND MaTroChuyen = @MaTroChuyen
      AND NgayNhan IS NULL;

    -- Lấy chi tiết câu hỏi gốc
    SELECT MaTroChuyen, MaNguoiGui, NgayGui, NoiDung
    FROM LX_HoiDap
    WHERE MaTroChuyen = @MaTroChuyen;

    -- Lấy danh sách người nhận
    SELECT MaNguoiNhan
    FROM LX_HoiDap_NguoiNhan
    WHERE MaTroChuyen = @MaTroChuyen;

    -- Lấy các câu trả lời nếu có
    SELECT MaTroChuyen, MaNguoiGui, NgayGui, NoiDung
    FROM LX_HoiDap
    WHERE MaLuongTroChuyen = @MaTroChuyen;
END 

-- tao cau tra loi
go
create or alter procedure SP_HoiDap_TaoTraLoi
    @MaTroChuyen UNIQUEIDENTIFIER,
    @MaNguoiGui int,
    @NoiDung nvarchar(4000)
    as
    begin
    insert into LX_HoiDap(MaLuongTroChuyen,MaNguoiGui,NoiDung,NgayGui)
    values(@MaTroChuyen,@MaNguoiGui,@NoiDung,GETDATE())

    -- Gọi SP_HoiDap_ChiTietHoiDap để trả toàn bộ thông tin 
    EXEC SP_HoiDap_ChiTietHoiDap @MaTroChuyen = @MaTroChuyen, @MaNguoiDung = @MaNguoiGui;
    end

-- lay danh sach cau hoi
go 
CREATE OR ALTER PROCEDURE SP_HoiDap_DanhSachCauHoi
    @MaNguoiDung INT,
    @role NVARCHAR(100),
    @pageNumber INT,
    @pageSize INT,
    @searchKey NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    -- Lưu kết quả lọc vào bảng tạm
    CREATE TABLE #TempResults (
        MaTroChuyen UNIQUEIDENTIFIER,
        MaNguoiGui INT,
        NoiDung NVARCHAR(MAX),
        NgayGui DATETIME
    );

    -- Sửa lại logic điều kiện lọc
    INSERT INTO #TempResults (MaTroChuyen, MaNguoiGui, NoiDung, NgayGui)
    SELECT MaTroChuyen, MaNguoiGui, NoiDung, NgayGui
    FROM LX_HoiDap
    WHERE MaLuongTroChuyen IS NULL
        AND (
            (@role = 'BanGiamHieu') -- Ban giám hiệu xem tất cả
            OR 
            (@role = 'HocVien' AND MaNguoiGui = @MaNguoiDung) -- Học viên chỉ xem câu hỏi của mình
            OR 
            (@role = 'GiaoVien' AND EXISTS ( -- Giáo viên chỉ xem câu hỏi được gửi đến mình
                SELECT 1
                FROM LX_HoiDap_NguoiNhan
                WHERE LX_HoiDap_NguoiNhan.MaTroChuyen = LX_HoiDap.MaTroChuyen
                    AND LX_HoiDap_NguoiNhan.MaNguoiNhan = @MaNguoiDung
            ))
        )
        AND (
            @searchKey IS NULL 
            OR NoiDung LIKE N'%' + @searchKey + '%'
        );

    -- Tổng số bản ghi
    SELECT COUNT(*) AS TotalItems FROM #TempResults;

    -- Dữ liệu phân trang
    SELECT *
    FROM #TempResults
    ORDER BY NgayGui DESC
    OFFSET (@pageNumber - 1) * @pageSize ROWS
    FETCH NEXT @pageSize ROWS ONLY;

    -- Xoá bảng tạm
    DROP TABLE #TempResults;
END
