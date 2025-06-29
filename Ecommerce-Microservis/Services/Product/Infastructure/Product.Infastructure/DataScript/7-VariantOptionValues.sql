--Mock Data For VariantOptionValues
INSERT INTO VariantOptionValues (Id, VariantOptionId, Value, CreatedDate, UpdatedDate)
VALUES
  (NEWID(), (SELECT Id FROM VariantOptions WHERE Name = 'Renk'), 'Gri', GETDATE(), GETDATE()),
  (NEWID(), (SELECT Id FROM VariantOptions WHERE Name = 'Renk'), 'Siyah', GETDATE(), GETDATE()),
  (NEWID(), (SELECT Id FROM VariantOptions WHERE Name = 'Renk'), 'Phantom Gray', GETDATE(), GETDATE()),
  (NEWID(), (SELECT Id FROM VariantOptions WHERE Name = 'Beden'), '42', GETDATE(), GETDATE());