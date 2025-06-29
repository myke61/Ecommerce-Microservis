--Mock Data For ProductVariantOptions
INSERT INTO ProductVariantOptions (Id, ProductVariantId, VariantOptionId, VariantOptionValueId, CreatedDate, UpdatedDate)
VALUES
  (NEWID(), (SELECT Id FROM ProductVariants WHERE Sku = 'IP13P-128GB-Gray'), (SELECT Id FROM VariantOptions WHERE Name = 'Renk'), (SELECT Id FROM VariantOptionValues WHERE Value = 'Gri'), GETDATE(), GETDATE()),
  (NEWID(), (SELECT Id FROM ProductVariants WHERE Sku = 'NAM270-42-Black'), (SELECT Id FROM VariantOptions WHERE Name = 'Renk'), (SELECT Id FROM VariantOptionValues WHERE Value = 'Siyah'), GETDATE(), GETDATE()),
  (NEWID(), (SELECT Id FROM ProductVariants WHERE Sku = 'SGS21-256GB-PhantomGray'), (SELECT Id FROM VariantOptions WHERE Name = 'Renk'), (SELECT Id FROM VariantOptionValues WHERE Value = 'Phantom Gray'), GETDATE(), GETDATE());