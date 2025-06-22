USE MASTER;

CREATE DATABASE CuaHangMayTinh
go
USE CuaHangMayTinh
go

CREATE TABLE DanhMuc (
    DanhMucID INT,
    TenDanhMuc NVARCHAR(100),
	CONSTRAINT PK_DanhMuc PRIMARY KEY(DanhMucID)
);

CREATE TABLE MT (
    MTID INT,
    TenSP NVARCHAR(255),
    MoTa NVARCHAR(MAX),
    Gia DECIMAL(18, 2),
    DanhMucID INT,
    SoLuongKho INT,
    ThuongHieu NVARCHAR(100),
    ViXuLy NVARCHAR(100),
    RAM NVARCHAR(50),
    LuuTru NVARCHAR(50),
    KichThuocManHinh NVARCHAR(50),
    MauSac NVARCHAR(50),
    DungLuongPin NVARCHAR(50),
	DuongDan varchar(100),
	CONSTRAINT PK_MT PRIMARY KEY(MTID),
    CONSTRAINT FK_MT_DanhMucID FOREIGN KEY (DanhMucID) REFERENCES DanhMuc(DanhMucID)
);

CREATE TABLE KhachHang (
    KhachHangID INT,
    TenKhachHang NVARCHAR(255),
    Email NVARCHAR(255),
	MatKhau CHAR(255),
    SoDienThoai NVARCHAR(50),
    DiaChi NVARCHAR(255),
	CONSTRAINT PK_KhachHang PRIMARY KEY(KhachHangID)
);

CREATE TABLE DanhGia (
    DanhGiaID INT IDENTITY(1,1) PRIMARY KEY,
    KhachHangID INT,
    MTID INT,
    DiemDanhGia INT CHECK (DiemDanhGia BETWEEN 1 AND 5),
    NoiDungDanhGia NVARCHAR(MAX),
    NgayDanhGia DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_DanhGia_KhachHang FOREIGN KEY (KhachHangID) REFERENCES KhachHang(KhachHangID),
    CONSTRAINT FK_DanhGia_MT FOREIGN KEY (MTID) REFERENCES MT(MTID)
);

CREATE TABLE DonHang (
    DonHangID INT,
    KhachHangID INT,
    NgayDatHang DATETIME DEFAULT GETDATE(),
    TongTien DECIMAL(18, 2),
    TrangThaiDonHang NVARCHAR(50),
    PhuongThucThanhToan NVARCHAR(50),
    DiaChiGiaoHang NVARCHAR(255),
	CONSTRAINT PK_DonHang PRIMARY KEY(DonHangID),
    CONSTRAINT FK_DonHang_KhachHangID FOREIGN KEY (KhachHangID) REFERENCES KhachHang(KhachHangID)
);

CREATE TABLE CTDonHang (
    DonHangID INT,
    MTID INT,
    SoLuong INT,
    Gia DECIMAL(18, 2),
	CONSTRAINT PK_CTDonHang PRIMARY KEY(DonHangID, MTID),
    CONSTRAINT FK_CTDonHang_DonHangID FOREIGN KEY (DonHangID) REFERENCES DonHang(DonHangID),
    CONSTRAINT FK_CTDonHang_MTID FOREIGN KEY (MTID) REFERENCES MT(MTID)
);

CREATE TABLE LIENHE
(
	IDLienHe int,
	KhachHangID int,
	NoiDung Text,
	CONSTRAINT PK_LIENHE PRIMARY KEY(IDLienHe, KhachHangID),
	CONSTRAINT FK_LIENHE_KHACHHANG FOREIGN KEY(KhachHangID) REFERENCES KhachHang(KhachHangID)
)
ALTER TABLE LIENHE
ALTER COLUMN NoiDung NVARCHAR(MAX);

INSERT INTO DanhMuc (DanhMucID, TenDanhMuc)
VALUES 
    (1, N'Laptop'),
    (2, N'Desktop'),
    (3, N'Tablet'),
    (4, N'Monitor'),
    (5, N'Accessories');

INSERT INTO MT (MTID, TenSP, MoTa, Gia, DanhMucID, SoLuongKho, ThuongHieu, ViXuLy, RAM, LuuTru, KichThuocManHinh, MauSac, DungLuongPin, DuongDan)
VALUES
    (1,  N'Laptop ABC',               N'Model 2023, Lightweight',                       15000000, 1, 26, N'Dell',   N'Intel Core i5-1135G7',     N'8GB',  N'256GB SSD', N'15.6 inches', N'Black', N'4500mAh',           'anh1.jpg'),
    (2,  N'Desktop XYZ',              N'High performance',                              12000000, 2, 35, N'HP',     N'Intel Core i7-11700',      N'16GB', N'512GB SSD', N'27 inches',   N'Silver', N'0mAh',             'anh9.jpg'),
    (3,  N'Tablet Z10',               N'Portable tablet with 10 inch display',          8000000,  3, 40, N'Samsung',N'Exynos 9611',             N'4GB',  N'64GB eMMC', N'10 inches',   N'White', N'6000mAh',           'anh13.jpg'),
    (4,  N'Monitor P24',              N'24 inch FHD display',                           3000000,  4, 35, N'LG',     N'TI DM368 display scaler',   N'256MB',N'8MB flash',N'24 inches',   N'Black', N'0mAh',             'anh16.jpg'),
    (5,  N'Wireless Mouse',           N'Ergonomic design',                              250000,   5, 50, N'Logitech',N'PixArt PMW3325 sensor',    N'Không áp dụng', N'Không áp dụng',     N'Không áp dụng',        N'Gray',  N'2×AAA batteries',   'anh20.jpg'),
    (6,  N'Laptop Pro X',             N'High-end business laptop',                      25000000, 1, 30, N'Apple',  N'Apple M1',                 N'16GB', N'512GB SSD', N'13 inches',   N'Space Gray',N'6000mAh',         'anh6.jpg'),
    (7,  N'Gaming Laptop Z',          N'Powerful gaming laptop',                        30000000, 1, 68, N'ASUS',   N'Intel Core i9-11900H',      N'32GB', N'1TB SSD',   N'17 inches',   N'Black', N'7500mAh',           'anh7.jpg'),
    (8,  N'Desktop Workstation',      N'High-performance for professionals',            18000000, 2, 62, N'HP',     N'Intel Xeon W-2245',        N'32GB', N'1TB SSD',   N'Không áp dụng',        N'Black', N'0mAh',             'anh12.jpg'),
    (9,  N'Desktop Basic',            N'Affordable desktop for everyday use',           8000000,  2, 35, N'Acer',   N'Intel Core i3-10100',       N'8GB',  N'256GB SSD', N'Không áp dụng',        N'White', N'0mAh',             'anh11.jpg'),
    (10, N'Tablet Mini',              N'Compact tablet for on-the-go use',              5000000,  3, 30, N'Apple',  N'A12 Bionic',               N'3GB',  N'64GB eMMC', N'7.9 inches',  N'Gold',  N'5000mAh',           'anh14.jpg'),
    (11, N'Professional Monitor',     N'Color-accurate display for designers',          7000000,  4, 40, N'Dell',   N'LSI MegaChips MCDP2800 scaler',N'512MB',N'4MB flash',N'27 inches', N'Silver',N'0mAh',             'anh17.jpg'),
    (12, N'UltraWide Monitor',       N'Curved monitor with ultrawide display',         10000000,  4, 48, N'Samsung',N'ARM Mali T86x GPU',        N'1GB',  N'16MB flash',N'34 inches',  N'Black', N'0mAh',             'anh18.jpg'),
    (13, N'Gaming Mouse G1',          N'Ergonomic mouse for gaming',                    800000,   5, 50, N'Razer',  N'PixArt PMW3389 sensor',     N'Không áp dụng', N'Không áp dụng',     N'Không áp dụng',        N'Green', N'1×AA battery',      'anh21.jpg'),
    (14, N'Mechanical Keyboard MK',   N'RGB backlit mechanical keyboard',               1500000,  5, 40, N'Corsair',N'Không áp dụng (no onboard processor)', N'Không áp dụng', N'Không áp dụng',     N'Không áp dụng',        N'Black', N'0mAh',             'anh22.jpg'),
    (15, N'Noise Cancelling Headphones',N'Over-ear headphones with noise cancellation',3000000,  5, 30, N'Sony',   N'Qualcomm QCC5124 Bluetooth',N'Không áp dụng', N'Không áp dụng',     N'Không áp dụng',        N'Black', N'700mAh',           'anh23.jpg'),
    (16, N'Laptop Ultrabook',         N'Slim and lightweight laptop',                   20000000, 1, 65, N'LG',     N'Intel Core i7-1165G7',      N'16GB', N'512GB SSD', N'14 inches',   N'Silver',N'5500mAh',         'anh8.jpg'),
    (17, N'Gaming Desktop Beast',     N'Ultimate gaming setup',                         40000000, 2, 55, N'Alienware',N'Intel Core i9-11900K',     N'64GB', N'2TB SSD',   N'Không áp dụng',        N'Black', N'0mAh',             'anh10.jpg'),
    (18, N'Tablet Pro',               N'Professional tablet for design',                15000000, 3, 30, N'Microsoft',N'Intel Core i5-1130G7',      N'8GB',  N'128GB SSD', N'12 inches',  N'Gray',  N'5800mAh',           'anh15.jpg'),
    (19, N'Curved Monitor Ultra HD',  N'High-resolution curved display',               9000000,  4, 37, N'Samsung',N'TI DM368 display scaler',   N'256MB',N'8MB flash',N'32 inches',  N'Black', N'0mAh',             'anh19.jpg'),
    (20, N'Portable SSD Drive',       N'High-speed external storage',                   2000000,  5, 60, N'Samsung',N'USB 3.1 controller',       N'Không áp dụng', N'1TB SSD',  N'Không áp dụng',        N'Blue',  N'0mAh',             'anh24.jpg'),
    (21, N'Laptop ZenBook 14',        N'Ultra-thin laptop for professionals',             20000000, 1, 12, N'ASUS',    N'Intel Core i7-1260P',   N'16GB', N'512GB SSD',   N'14 inches', N'Blue',  N'5700mAh', 'anh1.jpg'),
    (22, N'ROG Strix G15',             N'Gaming powerhouse with high refresh rate',        35000000, 1,  6, N'ASUS',    N'AMD Ryzen 9 5900HX',    N'32GB', N'1TB PCIe NVMe SSD', N'15.6 inches', N'Black', N'8000mAh', 'anh6.jpg'),
    (23, N'HP Spectre x360',           N'Convertible 2-in-1 with touch screen',            28000000, 1,  8, N'HP',      N'Intel Core i7-12700H',  N'16GB', N'1TB SSD',     N'13.3 inches', N'Silver',N'6500mAh', 'anh7.jpg'),
    (24, N'Acer Chromebook C',         N'Lightweight Chrome OS laptop',                   7000000,   1, 25, N'Acer',    N'Intel Celeron N4020',   N'4GB',  N'64GB eMMC',   N'11.6 inches', N'White', N'4000mAh', 'anh8.jpg'),
    (25, N'MSI Trident X',             N'Compact gaming desktop',                         25000000, 2,  5, N'MSI',     N'AMD Ryzen 7 5800X',     N'16GB', N'512GB SSD + 2TB HDD', N'Không tích hợp màn hình', N'Black', N'Không áp dụng', 'anh9.jpg'),
    (26, N'Intel NUC Mini PC',         N'Ultra-compact desktop PC',                        6000000,  2, 18, N'Intel',   N'Intel Core i5-10400T',   N'8GB',  N'256GB SSD',   N'Không tích hợp màn hình', N'Gray',  N'Không áp dụng', 'anh10.jpg'),
    (27, N'Dell Inspiron All-in-One',  N'23.8" touchscreen AIO desktop',                 12000000, 2,  7, N'Dell',    N'Intel Core i5-1135G7',   N'12GB', N'512GB SSD',   N'23.8 inches', N'White', N'Không áp dụng', 'anh11.jpg'),
    (28, N'Zotac ZBOX HTPC',           N'Home theater mini PC',                           8500000,  2, 10, N'Zotac',   N'Intel Core i3-10100',    N'8GB',  N'1TB HDD',     N'Không tích hợp màn hình', N'Black', N'Không áp dụng', 'anh12.jpg'),
    (29, N'Lenovo ThinkStation P340',  N'Professional CAD workstation',                  32000000, 2,  4, N'Lenovo',  N'Intel Xeon W-2295',      N'64GB', N'2TB SSD',     N'Không tích hợp màn hình', N'Silver',N'Không áp dụng', 'anh9.jpg'),
    (30, N'Samsung Galaxy Tab S8',     N'Flagship Android tablet',                       6500000,   3, 20, N'Samsung', N'Qualcomm Snapdragon 8 Gen 1', N'8GB',  N'128GB',   N'11 inches',  N'Gray',   N'8000mAh', 'anh13.jpg'),
    (31, N'Lenovo Tab M8',             N'Entry-level 8" Android tablet',                 3200000,   3, 15, N'Lenovo',  N'MediaTek Helio P22T',   N'2GB',  N'32GB',       N'8 inches',   N'Black',  N'5000mAh', 'anh14.jpg'),
    (32, N'Huawei MatePad 11',         N'11" productivity tablet',                      18000000,  3, 10, N'Huawei',  N'Qualcomm Snapdragon 888',N'6GB',  N'128GB',     N'11 inches',  N'Silver', N'7700mAh', 'anh15.jpg'),
    (33, N'HP Business Monitor 22',    N'Reliable 21.5" office display',                2500000,   4, 30, N'HP',      N'Realtek RTD2485 scaler',N'512MB',N'4MB flash', N'21.5 inches', N'Black', N'Không áp dụng', 'anh16.jpg'),
    (34, N'LG UltraGear 32GQ850',      N'32" QHD 165Hz gaming monitor',                 12000000,  4,  5, N'LG',      N'Realtek RTD2660 scaler',N'1GB',  N'16MB flash',N'32 inches',   N'Gray',   N'Không áp dụng', 'anh17.jpg'),
    (35, N'Dell P2422H',               N'24" Full HD ergonomic monitor',                10000000,  4,  8, N'Dell',    N'LSI MegaChips MCDP2800 scaler',N'512MB',N'4MB flash',N'24 inches', N'White', N'Không áp dụng', 'anh18.jpg'),
    (40, N'Asus ZenScreen 15.6 Portable',N'Portable USB-C monitor',                     5000000,   4, 10, N'ASUS',    N'Realtek RTL Controller', N'Không áp dụng',N'Không áp dụng',N'15.6 inches', N'Black', N'3000mAh', 'anh19.jpg'),
    (36, N'HyperX Cloud II Wireless',  N'Wireless gaming headset',                      2200000,   5, 40, N'HyperX',  N'Qualcomm QCC5125 chip', N'Không áp dụng',N'Không áp dụng',N'Không áp dụng', N'Black', N'600mAh', 'anh20.jpg'),
    (37, N'JBL Flip 5 Speaker',        N'Portable Bluetooth speaker',                   1500000,   5, 30, N'JBL',     N'JBL Connect+ chip',     N'Không áp dụng',N'Không áp dụng',N'Không áp dụng', N'Black', N'4800mAh', 'anh21.jpg'),
    (38, N'Anker PowerExpand 7-in-1 Hub',N'USB-C multiport adapter',                  800000,    5, 60, N'Anker',   N'Realtek RTS5411 PD controller',N'Không áp dụng',N'Không áp dụng',N'Không áp dụng', N'Gray',  N'Không áp dụng', 'anh22.jpg'),
    (39, N'Wacom Intuos Pro Small',    N'Graphics tablet for creators',                2200000,   5, 20, N'Wacom',   N'Wacom EMR pen controller',N'Không áp dụng',N'Không áp dụng',N'8.7×5.8 inches', N'Black', N'Không áp dụng', 'anh23.jpg');

INSERT INTO KhachHang (KhachHangID, TenKhachHang, Email, SoDienThoai, DiaChi, MatKhau)
VALUES 
    (1, N'Nguyen Van A', N'nguyenvana@gmail.com', N'0901234567', N'Hanoi','1'),
    (2, N'Tran Thi B', N'tranthib@gmail.com', N'0902345678', N'HCM City','1'),
    (3, N'Le Van C', N'levanc@gmail.com', N'0903456789', N'Danang','1'),
    (4, N'Pham Thi D', N'phamthid@gmail.com', N'0904567890', N'Hue','1'),
    (5, N'Hoang Van E', N'hoangvane@gmail.com', N'0905678901', N'Haiphong','1'),
	(6, N'Nguyen Thi F', N'nguyenthif@gmail.com', N'0906789012', N'Can Tho', '1'),
    (7, N'Tran Van G', N'tranvang@gmail.com', N'0907890123', N'Nha Trang', '1'),
    (8, N'Le Thi H', N'lethih@gmail.com', N'0908901234', N'Vung Tau', '1'),
    (9, N'Do Van I', N'dovani@gmail.com', N'0909012345', N'Quy Nhon', '1'),
    (10, N'Pham Van J', N'phamvanj@gmail.com', N'0910123456', N'Buon Ma Thuot', '1'),
	(11, N'Nguyen Van K', N'nguyenvank@gmail.com', N'0911234567', N'Thai Binh', '1'),
    (12, N'Tran Thi L', N'tranthil@gmail.com', N'0912345678', N'Thanh Hoa', '1'),
    (13, N'Le Van M', N'levanm@gmail.com', N'0913456789', N'Nam Dinh', '1'),
    (14, N'Pham Thi N', N'phamthin@gmail.com', N'0914567890', N'Bac Ninh', '1'),
    (15, N'Hoang Van O', N'hoangvano@gmail.com', N'0915678901', N'Lang Son', '1');

INSERT INTO DanhGia (KhachHangID, MTID, DiemDanhGia, NoiDungDanhGia)
VALUES 
	(1, 1, 5, N'Rất hài lòng với chất lượng sản phẩm!'),
	(2, 3, 4, N'Máy chạy tốt, thiết kế đẹp.'),
	(3, 11, 3, N'Hiệu năng tạm ổn, nhưng pin hơi yếu.'),
	(4, 15, 5, N'Phụ kiện chất lượng cao, rất ưng ý.'),
	(5, 2, 2, N'Máy hay bị đơ, không hài lòng lắm.'),
	(2, 40, 2, N'Bị lỗi vặt'),
	(3, 33, 3, N'Phù hợp nhu cầu cơ bản'),
	(5, 16, 3, N'Tản nhiệt kém'),
	(6, 31, 1, N'Không đáng giá tiền'),
	(6, 25, 5, N'Chất liệu rẻ tiền'),
	(1, 20, 4, N'Giá cả hợp lý'),
    (4, 7, 5, N'Thiết kế đẹp mắt'),
    (7, 22, 2, N'Máy hơi nóng'),
    (8, 28, 4, N'Âm thanh tốt'),
    (9, 13, 5, N'Hiệu năng vượt trội'),
    (2, 11, 4, N'Dễ sử dụng'),
    (3, 36, 1, N'Khó cài đặt'),
    (5, 29, 5, N'Giao hàng nhanh chóng'),
    (1, 23, 3, N'Không hài lòng về pin'),
    (10, 20, 5, N'Rất hài lòng'),
    (4, 20, 2, N'Không đáng tiền'),
    (7, 35, 4, N'Phù hợp nhu cầu'),
    (8, 15, 3, N'Chất lượng ổn định'),
    (9, 32, 5, N'Nên mua'),
    (2, 31, 4, N'Màn hình sắc nét'),
    (3, 28, 3, N'Thieu jack cắm'),
    (6, 30, 2, N'Webcam kém'),
    (5, 14, 5, N'Tản nhiệt tốt'),
    (1, 17, 4, N'Pin dùng lâu'),
    (10, 19, 5, N'Sạc nhanh'),
    (4, 13, 3, N'Phần mềm dễ xài'),
    (7, 18, 4, N'Chuột di chuyển mượt'),
    (8, 12, 5, N'Bàn phím thoải mái'),
    (9, 11, 2, N'Màu sắc nhạt'),
    (2, 12, 4, N'Cáp sạc tốt');

INSERT INTO DonHang (DonHangID, KhachHangID, NgayDatHang, TongTien, TrangThaiDonHang, PhuongThucThanhToan, DiaChiGiaoHang)
VALUES 
    (1, 1, GETDATE(), 15500000, N'Đang giao', N'Thanh toán khi nhận hàng', N'Hanoi'),
    (2, 2, GETDATE(), 12300000, N'Hoàn thành', N'Chuyển khoản', N'HCM City'),
    (3, 3, GETDATE(), 8100000, N'Bị huỷ', N'Chuyển khoản', N'Danang'),
    (4, 4, GETDATE(), 3050000, N'Đang giao', N'Thanh toán khi nhận hàng', N'Hue'),
    (5, 5, GETDATE(), 250000, N'Đang giao', N'Chuyển khoản', N'Haiphong'),
	(6, 6, GETDATE(), 4500000, N'Hoàn thành', N'Chuyển khoản', N'Can Tho'),
    (7, 7, GETDATE(), 11200000, N'Đang giao', N'Chuyển khoản', N'Nha Trang'),
    (8, 8, GETDATE(), 9750000, N'Đang giao', N'Chuyển khoản', N'Vung Tau'),
    (9, 9, GETDATE(), 3100000, N'Hoàn thành', N'Thanh toán khi nhận hàng', N'Quy Nhon'),
    (10, 10, GETDATE(), 15800000, N'Đang giao', N'Chuyển khoản', N'Buon Ma Thuot'),
	(11, 1, GETDATE(), 7200000, N'Hoàn thành', N'Thanh toán khi nhận hàng', N'Hanoi'),
    (12, 3, GETDATE(), 9100000, N'Đang giao', N'Chuyển khoản', N'Danang'),
    (13, 6, GETDATE(), 6400000, N'Bị huỷ', N'Chuyển khoản', N'Can Tho'),
    (14, 8, GETDATE(), 5300000, N'Hoàn thành', N'Chuyển khoản', N'Vung Tau'),
    (15, 10, GETDATE(), 14800000, N'Đang giao', N'Chuyển khoản', N'Buon Ma Thuot'),
	(16, 11, GETDATE(), 6700000, N'Đang giao', N'Thanh toán khi nhận hàng', N'Thai Binh'),
    (17, 12, GETDATE(), 8400000, N'Hoàn thành', N'Chuyển khoản', N'Thanh Hoa'),
    (18, 13, GETDATE(), 9100000, N'Bị huỷ', N'Chuyển khoản', N'Nam Dinh'),
    (19, 14, GETDATE(), 4000000, N'Đang giao', N'Chuyển khoản', N'Bac Ninh'),
    (20, 15, GETDATE(), 7500000, N'Đang giao', N'Thanh toán khi nhận hàng', N'Lang Son'),
    (21, 2, GETDATE(), 1230000, N'Hoàn thành', N'Chuyển khoản', N'HCM City'),
    (22, 4, GETDATE(), 850000, N'Đang giao', N'Chuyển khoản', N'Hue'),
    (23, 6, GETDATE(), 7800000, N'Bị huỷ', N'Thanh toán khi nhận hàng', N'Can Tho'),
    (24, 8, GETDATE(), 9400000, N'Hoàn thành', N'Chuyển khoản', N'Vung Tau'),
    (25, 10, GETDATE(), 10100000, N'Đang giao', N'Chuyển khoản', N'Buon Ma Thuot');

INSERT INTO CTDonHang (DonHangID, MTID, SoLuong, Gia)
VALUES 
    (1, 1, 1, 15000000),
    (2, 2, 1, 12000000),
    (3, 3, 1, 8000000),
    (4, 4, 1, 3000000),
    (5, 5, 1, 250000),
	(6, 6, 1, 4500000),
    (7, 7, 1, 11000000),
    (8, 8, 1, 9700000),
    (9, 9, 1, 3000000),
    (10, 10, 1, 15800000),
    (11, 1, 1, 7200000),
    (12, 3, 1, 9100000),
    (13, 6, 1, 6400000),
	(14, 2, 1, 5300000),
    (15, 5, 1, 14800000),
    (16, 3, 1, 6700000),
    (17, 6, 1, 8400000),
    (18, 1, 1, 9100000),
    (19, 4, 1, 4000000),
    (20, 2, 1, 7500000),
    (21, 5, 1, 1230000),
    (22, 3, 1, 850000),
    (23, 1, 1, 7800000),
    (24, 6, 1, 9400000),
    (25, 4, 1, 10100000);




SELECT * FROM DanhMuc
SELECT * FROM MT
SELECT * FROM KhachHang
SELECT * FROM DonHang
SELECT * FROM CTDonHang
SELECT * FROM LIENHE




CREATE OR ALTER PROCEDURE CapNhatKhoDuaTrenDonHang 
    @DonHangID INT
AS
BEGIN
    DECLARE @MTID INT, @SoLuong INT, @SoLuongKho INT;

    -- Cursor CTDonHang
    DECLARE cur CURSOR FOR
    SELECT MTID, SoLuong
    FROM CTDonHang
    WHERE DonHangID = @DonHangID;

    OPEN cur;
    FETCH NEXT FROM cur INTO @MTID, @SoLuong;

    WHILE @@FETCH_STATUS = 0
    BEGIN

        SELECT @SoLuongKho = SoLuongKho
        FROM MT
        WHERE MTID = @MTID;

        IF @SoLuongKho < @SoLuong
        BEGIN
            PRINT 'KHONG DU SO LUONG MTID ' + CAST(@MTID AS NVARCHAR(10));

            ROLLBACK TRANSACTION; 
            BREAK;
        END
        ELSE
        BEGIN

            UPDATE MT
            SET SoLuongKho = SoLuongKho - @SoLuong
            WHERE MTID = @MTID;

            PRINT 'SO LUONG DUOC CAP NHAT MTID ' + CAST(@MTID AS NVARCHAR(10));
        END

        FETCH NEXT FROM cur INTO @MTID, @SoLuong;
    END

    CLOSE cur;
    DEALLOCATE cur;
END;
--test
exec CapNhatKhoDuaTrenDonHang 6