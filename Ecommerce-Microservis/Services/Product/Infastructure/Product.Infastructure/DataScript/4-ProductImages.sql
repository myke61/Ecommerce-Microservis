--Mock Data For ProductImages
INSERT INTO ProductImages (Id, ProductId, ImageUrl, DisplayOrder, IsMain, CreatedDate, UpdatedDate)
VALUES
  (NEWID(), (SELECT Id FROM Products WHERE Code = 'IP13P-128GB'), 'https://example.com/iphone13pro.jpg', 1, 1, GETDATE(), GETDATE()),
  (NEWID(), (SELECT Id FROM Products WHERE Code = 'NAM270-42'), 'https://example.com/nikeairmax270.jpg', 1, 1, GETDATE(), GETDATE()),
  (NEWID(), (SELECT Id FROM Products WHERE Code = 'SGS21-256GB'), 'https://example.com/galaxys21.jpg', 1, 1, GETDATE(), GETDATE());