--Mock Data for Brands
INSERT INTO Brands (Id, Name, Description, LogoUrl, CreatedDate, UpdatedDate)
VALUES
  (NEWID(), 'Apple', 'Apple Inc. teknoloji þirketi', 'https://example.com/apple-logo.png', GETDATE(), GETDATE()),
  (NEWID(), 'Nike', 'Nike spor giyim markasý', 'https://example.com/nike-logo.png', GETDATE(), GETDATE()),
  (NEWID(), 'Samsung', 'Samsung elektronik markasý', 'https://example.com/samsung-logo.png', GETDATE(), GETDATE());