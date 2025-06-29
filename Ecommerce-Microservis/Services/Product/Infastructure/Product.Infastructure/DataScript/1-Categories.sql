--Mock Data for Categories
INSERT INTO Categories (Id, Name, Slug, Description, ParentCategoryId, CreatedDate, UpdatedDate)
VALUES
  (NEWID(), 'Elektronik', 'elektronik', 'Elektronik ürünler kategorisi', NULL, GETDATE(), GETDATE()),
  (NEWID(), 'Moda', 'moda', 'Giyim ve aksesuarlar kategorisi', NULL, GETDATE(), GETDATE()),
  (NEWID(), 'Ev & Yaþam', 'ev-yasam', 'Ev eþyalarý ve yaþam ürünleri', NULL, GETDATE(), GETDATE());