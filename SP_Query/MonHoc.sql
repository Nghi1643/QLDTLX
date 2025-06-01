-- Lấy 1 môn học theo MaMon
CREATE OR ALTER PROCEDURE SP_MonHoc_GetMonHocByMaMon
    @MaMon SMALLINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT *
    FROM LX_MonHoc
    WHERE MaMon = @MaMon
END
GO

-- Lấy tất cả môn học + tìm kiếm phân trang
CREATE OR ALTER PROCEDURE SP_MonHoc_GetAllMonHoc
    @pageNumber INT,
    @pageSize INT,
    @searchKey NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    -- Số lượng item
    SELECT COUNT(*)
    FROM LX_MonHoc
    WHERE @searchKey IS NULL OR TenMon LIKE N'%' + @searchKey + N'%'
    -- Danh sách phân trang
    SELECT *
    FROM LX_MonHoc
    WHERE @searchKey IS NULL OR TenMon LIKE N'%' + @searchKey + N'%'
    ORDER BY TenMon
    OFFSET (@pageNumber - 1) * @pageSize ROWS
    FETCH NEXT @pageSize ROWS ONLY;
END
GO

-- Tạo môn học
CREATE OR ALTER PROCEDURE SP_MonHoc_TaoMonHoc
    @TenMon NVARCHAR(150),
    @KyHieu NVARCHAR(10) = NULL,
    @MoTa NVARCHAR(4000) = NULL,
    @HinhThucHoc BIT,
    @ThucHanh BIT,
    @ChamDiem BIT = NULL,
    @SuDung BIT = NULL,
    @TepKemTheo NVARCHAR(260) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @OutputTable TABLE (MaMon SMALLINT);
    DECLARE @MaMon SMALLINT;
    BEGIN TRY
        -- Kiểm tra trùng tên trước khi insert
        IF EXISTS (SELECT 1 FROM LX_MonHoc WHERE TenMon = @TenMon)
        BEGIN
            RAISERROR(N'Tên môn học đã tồn tại. Vui lòng chọn tên khác.', 16, 1);
            RETURN;
        END
        INSERT INTO LX_MonHoc(TenMon, KyHieu, MoTa, HinhThucHoc, ThucHanh, ChamDiem, SuDung, TepKemTheo)
        OUTPUT INSERTED.MaMon INTO @OutputTable(MaMon)
        VALUES(@TenMon, @KyHieu, @MoTa, @HinhThucHoc, @ThucHanh, @ChamDiem, @SuDung, @TepKemTheo);

        -- Lấy MaMon vừa tạo
        SELECT TOP 1 @MaMon = MaMon FROM @OutputTable;

        -- Trả về môn học vừa tạo
        EXEC SP_MonHoc_GetMonHocByMaMon @MaMon;
    END TRY
    BEGIN CATCH
        THROW
    END CATCH
END
GO

-- Cập nhật toàn bộ môn học
CREATE OR ALTER PROCEDURE SP_MonHoc_CapNhatMonHoc
    @MaMon SMALLINT,
    @TenMon NVARCHAR(150),
    @KyHieu NVARCHAR(10) = NULL,
    @MoTa NVARCHAR(4000) = NULL,
    @HinhThucHoc BIT,
    @ThucHanh BIT,
    @ChamDiem BIT = NULL,
    @SuDung BIT = NULL,
    @TepKemTheo NVARCHAR(260) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Kiểm tra môn học có tồn tại không
        IF NOT EXISTS (SELECT 1 FROM LX_MonHoc WHERE MaMon = @MaMon)
        BEGIN
            RAISERROR(N'Môn học không tồn tại.', 16, 1);
            RETURN;
        END

        -- Kiểm tra trùng tên (trừ chính nó)
        IF EXISTS (SELECT 1 FROM LX_MonHoc WHERE TenMon = @TenMon AND MaMon != @MaMon)
        BEGIN
            RAISERROR(N'Tên môn học đã tồn tại. Vui lòng chọn tên khác.', 16, 1);
            RETURN;
        END

        -- Cập nhật môn học
        UPDATE LX_MonHoc
        SET TenMon = @TenMon,
            KyHieu = @KyHieu,
            MoTa = @MoTa,
            HinhThucHoc = @HinhThucHoc,
            ThucHanh = @ThucHanh,
            ChamDiem = @ChamDiem,
            SuDung = @SuDung,
            TepKemTheo = @TepKemTheo
        WHERE MaMon = @MaMon

        -- Trả về môn học đã cập nhật
        EXEC SP_MonHoc_GetMonHocByMaMon @MaMon;
    END TRY
    BEGIN CATCH
        THROW
    END CATCH
END
GO

-- Cập nhật một phần môn học
CREATE OR ALTER PROCEDURE SP_MonHoc_CapNhatMotPhanMonHoc
    @MaMon SMALLINT,
    @TenMon NVARCHAR(150) = NULL,
    @KyHieu NVARCHAR(10) = NULL,
    @MoTa NVARCHAR(4000) = NULL,
    @HinhThucHoc BIT = NULL,
    @ThucHanh BIT = NULL,
    @ChamDiem BIT = NULL,
    @SuDung BIT = NULL,
    @TepKemTheo NVARCHAR(260) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Kiểm tra môn học có tồn tại không
        IF NOT EXISTS (SELECT 1 FROM LX_MonHoc WHERE MaMon = @MaMon)
        BEGIN
            RAISERROR(N'Môn học không tồn tại.', 16, 1);
            RETURN;
        END

        -- Kiểm tra trùng tên nếu có cập nhật tên (trừ chính nó)
        IF @TenMon IS NOT NULL AND EXISTS (SELECT 1 FROM LX_MonHoc WHERE TenMon = @TenMon AND MaMon != @MaMon)
        BEGIN
            RAISERROR(N'Tên môn học đã tồn tại. Vui lòng chọn tên khác.', 16, 1);
            RETURN;
        END

        -- Cập nhật các trường được chỉ định
        UPDATE LX_MonHoc
        SET TenMon = COALESCE(@TenMon, TenMon),
            KyHieu = COALESCE(@KyHieu, KyHieu),
            MoTa = COALESCE(@MoTa, MoTa),
            HinhThucHoc = COALESCE(@HinhThucHoc, HinhThucHoc),
            ThucHanh = COALESCE(@ThucHanh, ThucHanh),
            ChamDiem = COALESCE(@ChamDiem, ChamDiem),
            SuDung = COALESCE(@SuDung, SuDung),
            TepKemTheo = COALESCE(@TepKemTheo, TepKemTheo)
        WHERE MaMon = @MaMon

        -- Trả về môn học đã cập nhật
        EXEC SP_MonHoc_GetMonHocByMaMon @MaMon;
    END TRY
    BEGIN CATCH
        THROW
    END CATCH
END
GO