--Mock Data for Brands
INSERT INTO Brands (Id, Name, Description, LogoUrl, CreatedDate, UpdatedDate)
VALUES
  (NEWID(), 'Apple', 'Apple Inc. teknoloji �irketi', 'https://example.com/apple-logo.png', GETDATE(), GETDATE()),
  (NEWID(), 'Nike', 'Nike spor giyim markas�', 'https://example.com/nike-logo.png', GETDATE(), GETDATE()),
  (NEWID(), 'Samsung', 'Samsung elektronik markas�', 'https://example.com/samsung-logo.png', GETDATE(), GETDATE());