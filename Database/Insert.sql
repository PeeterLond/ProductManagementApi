INSERT INTO AppSchema.Store (StoreName)
    VALUES ('Sõpruse Rimi'), ('Mustakivi Prisma'), ('Vilde Maxima'), ('Tondi Selver');

INSERT INTO AppSchema.ParentGroup (ParentGroupName)
    VALUES ('Pagaritooted'), ('Lihatooted'), ('Kuivained');

INSERT INTO AppSchema.SubGroup (SubGroupName, ParentGroupId)
    VALUES ('Saiad', 1), ('Leivad', 1), ('Värske Liha', 2), ('Viinerid', 2), ('Süldid', 2), ('Pasta', 3), ('Pudrud', 3), ('Riis', 3);

INSERT INTO AppSchema.Product (ProductName, ProductAdded, Price, PriceVat, SubGroupId, Active)
    VALUES ('Kodune hakkliha', GETDATE(), 3.51, 4.39, 3, 1), 
    ('Suitsuviiner', GETDATE(), 1.23, 1.54, 4, 1), 
    ('Mitmevilja röst', GETDATE(), 1.66, 2.08, 1, 1), 
    ('Tallinna peenleib', GETDATE(), 1.32, 1.65, 2, 1),
    ('Täisterapasta Fusilli', GETDATE(), 1.32, 1.65, 6, 1),
    ('Tatrahelbed', GETDATE(), 2.62, 3.27, 7, 1);

INSERT INTO AppSchema.StoreProduct
    VALUES (1, 1, 10), 
    (1, 3, 15),
    (2, 2, 25),
    (2, 4, 10),
    (3, 1, 8),
    (3, 5, 50),
    (4, 6, 50),
    (4, 3, 50),
    (4, 2, 15);