--Mock Data For ProductVariants
INSERT INTO ProductVariants (Id, ProductId, Sku, Price, StockQuantity, CreatedDate, UpdatedDate)
VALUES
  (NEWID(), (SELECT Id FROM Products WHERE Code = 'IP13P-128GB'), 'IP13P-128GB-Gray', 999.99, 50, GETDATE(), GETDATE()),
  (NEWID(), (SELECT Id FROM Products WHERE Code = 'NAM270-42'), 'NAM270-42-Black', 129.99, 100, GETDATE(), GETDATE()),
  (NEWID(), (SELECT Id FROM Products WHERE Code = 'SGS21-256GB'), 'SGS21-256GB-PhantomGray', 799.99, 30, GETDATE(), GETDATE());