--Mock Data For Products
INSERT INTO Products (Id, Name, Code, Description, Slug, IsDeleted, BrandId, CategoryId, CreatedDate, UpdatedDate)
VALUES
  (NEWID(), 'iPhone 13 Pro', 'IP13P-128GB', 'Apple iPhone 13 Pro 128GB', 'iphone-13-pro', 0, (SELECT Id FROM Brands WHERE Name = 'Apple'), (SELECT Id FROM Categories WHERE Name = 'Elektronik'), GETDATE(), GETDATE()),
  (NEWID(), 'Nike Air Max 270', 'NAM270-42', 'Nike Air Max 270 Spor Ayakkabý', 'nike-air-max-270', 0, (SELECT Id FROM Brands WHERE Name = 'Nike'), (SELECT Id FROM Categories WHERE Name = 'Moda'), GETDATE(), GETDATE()),
  (NEWID(), 'Samsung Galaxy S21', 'SGS21-256GB', 'Samsung Galaxy S21 256GB', 'samsung-galaxy-s21', 0, (SELECT Id FROM Brands WHERE Name = 'Samsung'), (SELECT Id FROM Categories WHERE Name = 'Elektronik'), GETDATE(), GETDATE());