--tao cau hoi khao sat
go
create or alter procedure SP_KhaoSat_TaoCauHoi
    @nhom tinyint,
    @ngayBatDau datetime2,
    @ngayKetThuc datetime2,
    @noiDung nvarchar(500),
    @suDung bit
as
begin
    set nocount on;
	insert into LX_KhaoSat_CauHoi(Nhom,NgayBatDau,NgayKetThuc,NoiDung,SuDung)
	values(@nhom,@ngayBatDau,@ngayKetThuc,@noiDung,@suDung)
end

--cap nhat toan bo cau hoi khao sat
go
create or alter procedure SP_KhaoSat_CapNhatCauHoi
    @maCauHoi smallint,
    @nhom tinyint,
    @ngayBatDau datetime2,
    @ngayKetThuc datetime2,
    @noiDung nvarchar(500),
    @suDung bit
as
begin
    set nocount on;
    update LX_KhaoSat_CauHoi
    set Nhom = @nhom,
        NgayBatDau = @ngayBatDau,
        NgayKetThuc = @ngayKetThuc,
        NoiDung = @noiDung,
        SuDung = @suDung
    where MaCauHoi = @maCauHoi
    -- Trả về 1 nếu update thành công, 0 nếu không tìm thấy
    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END AS Success;
end

--cap nhat trang thai su dung
go
create or alter procedure SP_KhaoSat_CapNhatTrangThaiSuDung
    @maCauHoi int
as
begin
    set nocount on;
    update LX_KhaoSat_CauHoi
    -- tự đảo ngược giá trị hiện tại mà k càn client gửi lên
    set SuDung = case when SuDung=1 then 0 else 1 end
    where MaCauHoi = @maCauHoi
    -- Trả về 1 nếu update thành công, 0 nếu không tìm thấy
    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END AS Success;
end

-- thông báo đánh giá tổng kết khóa học
-- danh sach cau hoi khao sat (đánh giá 1 lần/(giáo viên+môn học))
go
create or alter procedure SP_KhaoSat_DanhSachCauHoi_TongKet
    @MaHocVien int,
    @DaTraLoi bit -- 1: da tra loi, 2: chua tra loi
as
begin   
    set nocount on;
    -- tao chi muc cho LX_QuaTrinhDaoTao(MaHocVien)
    if not exists (select 1 from sys.indexes where name = 'IX_QuaTrinhDaoTao_MaHocVien')
        CREATE INDEX IX_QuaTrinhDaoTao_MaHocVien ON LX_QuaTrinhDaoTao(MaHocVien);

    select distinct-- gop ket qua thua
    ch.MaCauHoi, ch.Nhom, ch.NoiDung,
    ch.NgayBatDau, ch.NgayKetThuc,
    gv.MaGiaoVien, gv.HoTen,
    mh.MaMon, mh.TenMon
    from LX_QuaTrinhDaoTao qtdt 
    join LX_GiaoVien gv on qtdt.MaGiaoVien=gv.MaGiaoVien
    join LX_GiaoVien_MonHoc gvmh on gvmh.MaGiaoVien=gv.MaGiaoVien
    join LX_MonHoc mh on mh.MaMon= gvmh.MaMon
    cross join LX_KhaoSat_CauHoi ch -- gui 1 cau hoi cho tat ca cac ban ghi hop le
    where qtdt.MaHocVien=@MaHocVien
    and ch.SuDung=1
    and (ch.NgayBatDau is null or getdate()>=ch.NgayBatDau)
    and (ch.NgayKetThuc is null or getdate()<= ch.NgayKetThuc)
    and qtdt.Diem is not null -- co diem = da hoc xong
    and (@DaTraLoi=1 and exists(select 1 --lay cau hoi da tra loi
                                   from LX_KhaoSat_TraLoi tl
                                   where tl.MaCauHoi=ch.MaCauHoi
                                   and tl.MaHocVien=@MaHocVien
                                   and tl.MaGiaoVien=gv.MaGiaoVien)
        or @DaTraLoi=0 and not exists(select 1 --lay cau hoi chua tra loi
                                          from LX_KhaoSat_TraLoi tl
                                          where tl.MaCauHoi=ch.MaCauHoi
                                          and tl.MaHocVien=@MaHocVien
                                          and tl.MaGiaoVien=gv.MaGiaoVien)
         )
    order by ch.NgayKetThuc asc -- cau hoi sap ket thuc leen truoc
end
-- thông báo đánh giá từng buổi 
-- danh sach cau hoi khao sat (đánh giá 1 lần/1 buổi học/(giáo viên+môn học))
go
create or alter procedure SP_KhaoSat_DanhSachCauHoi_TungBuoi
    @MaHocVien int,
    @DaTraLoi bit -- 1: da tra loi, 0: chua tra loi
as
begin   
    set nocount on;
    -- tao chi muc cho LX_QuaTrinhDaoTao(MaHocVien)
    if not exists (select 1 from sys.indexes where name = 'IX_QuaTrinhDaoTao_MaHocVien')
        CREATE INDEX IX_QuaTrinhDaoTao_MaHocVien ON LX_QuaTrinhDaoTao(MaHocVien);

    select distinct
    ch.MaCauHoi, ch.Nhom, ch.NoiDung, ch.NgayBatDau, ch.NgayKetThuc,
    gv.MaGiaoVien, gv.HoTen,
    mh.MaMon, mh.TenMon,
    ldt.MaLich, ldt.MaLop
    from LX_QuaTrinhDaoTao qtdt 
    join LX_GiaoVien gv on qtdt.MaGiaoVien=gv.MaGiaoVien
    join LX_GiaoVien_MonHoc gvmh on gvmh.MaGiaoVien=gv.MaGiaoVien
    join LX_MonHoc mh on mh.MaMon=gvmh.MaMon
    join LX_HocVien_Lop hvl on hvl.MaHocVien=@MaHocVien
    join LX_LichDaoTao ldt on ldt.MaLop=hvl.MaLop 
        and ldt.MaGiaoVien=gv.MaGiaoVien 
        and ldt.MaMon=mh.MaMon
    cross join LX_KhaoSat_CauHoi ch
    where qtdt.MaHocVien=@MaHocVien
    and ch.SuDung=1
    and (ch.NgayBatDau is null or getdate()>=ch.NgayBatDau)
    and (ch.NgayKetThuc is null or getdate()<=ch.NgayKetThuc)
    and ldt.KetThuc < GETDATE() -- Chỉ lấy buổi học đã kết thúc
    and (@DaTraLoi=1 and exists(select 1 --lay cau hoi da tra loi
                               from LX_KhaoSat_TraLoi tl
                               where tl.MaCauHoi=ch.MaCauHoi
                               and tl.MaHocVien=@MaHocVien
                               and tl.MaGiaoVien=gv.MaGiaoVien)
        or @DaTraLoi=0 and not exists(select 1 --lay cau hoi chua tra loi
                                     from LX_KhaoSat_TraLoi tl
                                     where tl.MaCauHoi=ch.MaCauHoi
                                     and tl.MaHocVien=@MaHocVien
                                     and tl.MaGiaoVien=gv.MaGiaoVien)
       )
    order by ch.NgayKetThuc asc
end
-- Thêm stored procedure trả lời khảo sát
go
create or alter procedure sp_khaosat_traloi
    @maCauHoi smallint,
    @maHocVien int=null,
    @maGiaoVien int,
    @diem tinyint,
    @yKienKhac nvarchar(500) = null
as
begin
    set nocount on;
    
    -- Kiểm tra nếu đã tồn tại câu trả lời thì báo lỗi
    if exists (select 1 from LX_KhaoSat_TraLoi 
               where MaCauHoi = @maCauHoi and MaHocVien = @maHocVien and MaGiaoVien = @maGiaoVien)
    begin
        -- Trả về 0 nếu đã tồn tại câu trả lời
        return 0;
    end
    else
    begin
        -- Nếu chưa có câu trả lời thì thêm mới
        insert into LX_KhaoSat_TraLoi(MaCauHoi, MaHocVien, MaGiaoVien, Diem, YKienKhac)
        values(@maCauHoi, @maHocVien, @maGiaoVien, @diem, @yKienKhac);
        
        -- Trả về 1 nếu thành công
        return 1;
    end
end
-- Lấy kết quả khảo sát đánh giá theo câu hỏi (nếu có) hoặc tất cả 
go
create or alter procedure SP_KhaoSat_KetQuaDanhGia
    @maCauHoi smallint = null -- tham số tùy chọn
as
begin
    set nocount on;
    
    select 
        gv.MaGiaoVien,
        gv.HoTen,
        gv.AnhChanDung ,
        COUNT(tl.MaCauHoi) as SoLuotDanhGia,
        AVG(CAST(tl.Diem as float)) as DiemTrungBinh
    from LX_KhaoSat_TraLoi tl
    join LX_GiaoVien gv on tl.MaGiaoVien = gv.MaGiaoVien
    where tl.Diem is not null
      and (@maCauHoi is null or tl.MaCauHoi = @maCauHoi)
    group by gv.MaGiaoVien, gv.HoTen, gv.AnhChanDung
    order by DiemTrungBinh desc;
end