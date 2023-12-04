INSERT INTO AppSchema.Store (StoreName)
    VALUES ('Sõpruse Rimi'), ('Mustakivi Prisma'), ('Vilde Maxima'), ('Tondi Selver');

INSERT INTO AppSchema.ParentGroup (ParentGroupName)
    VALUES ('Pagaritooted'), ('Lihatooted'), ('Kuivained');

INSERT INTO AppSchema.SubGroup (SubGroupName, ParentGroupId)
    VALUES ('Saiad', 1), ('Leivad', 1), ('Värske Liha', 2), ('Viinerid', 2), ('Süldid', 2), ('Pasta', 3), ('Pudrud', 3), ('Riis', 3);

INSERT INTO AppSchema.Product (ProductName, ProductAdded, Price, PriceVat, Vat, SubGroupId, Active)
    VALUES ('Kodune hakkliha', GETDATE(), 3.51, 4.39, 20, 3, 1), 
    ('Suitsuviiner', GETDATE(), 1.23, 1.54, 20, 4, 1), 
    ('Mitmevilja röst', GETDATE(), 1.66, 2.08, 20, 1, 1), 
    ('Tallinna peenleib', GETDATE(), 1.32, 1.65, 20, 2, 1),
    ('Täisterapasta Fusilli', GETDATE(), 1.32, 1.65, 20, 6, 1),
    ('Tatrahelbed', GETDATE(), 2.62, 3.27, 20, 7, 1);

INSERT INTO AppSchema.StoreProduct
    VALUES (1, 1, 10), 
	(1, 6, 50),
    (1, 3, 15),
    (2, 2, 25),
    (2, 4, 10),
    (3, 1, 8),
    (3, 5, 50),
	(3, 3, 15),
    (4, 6, 50),
	(4, 5, 40),
	(4, 4, 10),
    (4, 3, 50),
    (4, 2, 15),
	(4, 1, 4);