--Mock Data for Categories
INSERT INTO Categories (Id, Name, Slug, Description, ParentCategoryId, CreatedDate, UpdatedDate)
VALUES
  (NEWID(), 'Elektronik', 'elektronik', 'Elektronik �r�nler kategorisi', NULL, GETDATE(), GETDATE()),
  (NEWID(), 'Moda', 'moda', 'Giyim ve aksesuarlar kategorisi', NULL, GETDATE(), GETDATE()),
  (NEWID(), 'Ev & Ya�am', 'ev-yasam', 'Ev e�yalar� ve ya�am �r�nleri', NULL, GETDATE(), GETDATE());