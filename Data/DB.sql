INSERT INTO dbo.Folders(Id,DisplayName,ParentId) VALUES ('ab4d78b5-aa58-4c92-b0be-a9aac30dd278','folder 1', NULL);
INSERT INTO dbo.Folders(Id,DisplayName,ParentId) VALUES ('db211e24-51d1-4b4d-aed9-d00d30540b05','folder 1 - 1','ab4d78b5-aa58-4c92-b0be-a9aac30dd278');
INSERT INTO dbo.Folders(Id,DisplayName,ParentId) VALUES ('59722347-d091-46e7-8aab-9664142f6b93','folder 1 - 2','ab4d78b5-aa58-4c92-b0be-a9aac30dd278');
INSERT INTO dbo.Folders(Id,DisplayName,ParentId) VALUES ('f2c59e50-bbb6-4396-9fe8-b9156d342362','folder 1 - 2 - 1','59722347-d091-46e7-8aab-9664142f6b93');
GO

INSERT INTO dbo.[File](Id,DisplayName,FileName,FolderId) VALUES ('836e82d7-6735-4019-95d6-08d96fb732b6','Sample1','C:\filestore\c0af17b2-5849-4b46-ae35-5f919f332749.jpg','ab4d78b5-aa58-4c92-b0be-a9aac30dd278');
INSERT INTO dbo.[File](Id,DisplayName,FileName,FolderId) VALUES ('8b13a955-1688-4bf8-9f29-08d96fdbfebb','Sample1','C:\filestore\a51d7e2f-1cb3-43e9-a2e0-bb0346d5b3cc.jpg','ab4d78b5-aa58-4c92-b0be-a9aac30dd278');
INSERT INTO dbo.[File](Id,DisplayName,FileName,FolderId) VALUES ('1af30dcb-b6b0-4b06-9f2a-08d96fdbfebb','Sample1','C:\filestore\26be6f99-9e1f-4dd8-8868-c059364bee70.jpg','ab4d78b5-aa58-4c92-b0be-a9aac30dd278');
INSERT INTO dbo.[File](Id,DisplayName,FileName,FolderId) VALUES ('dbdb61f6-ae99-49a1-9f2b-08d96fdbfebb','Sample1','C:\filestore\3a9fcaed-8e94-4134-b683-183293267d4c.jpg','ab4d78b5-aa58-4c92-b0be-a9aac30dd278');
INSERT INTO dbo.[File](Id,DisplayName,FileName,FolderId) VALUES ('56793059-ec6e-4ab6-9f2c-08d96fdbfebb','Sample1','C:\filestore\a876e97f-75f9-4ef8-8f43-1481ccf2334e.jpg','ab4d78b5-aa58-4c92-b0be-a9aac30dd278');
INSERT INTO dbo.[File](Id,DisplayName,FileName,FolderId) VALUES ('deea60f4-f66b-4bde-6a7b-08d96fdc5f93','Sample1','C:\filestore\d732bc97-5b02-475b-8a88-efdb12399549.jpg','ab4d78b5-aa58-4c92-b0be-a9aac30dd278');
INSERT INTO dbo.[File](Id,DisplayName,FileName,FolderId) VALUES ('f916d7be-98b5-4c0b-d658-08d970a67374','Sample1','C:\filestore\af98148e-367f-4e19-8c67-7dcc1e112027.jpg','db211e24-51d1-4b4d-aed9-d00d30540b05');
INSERT INTO dbo.[File](Id,DisplayName,FileName,FolderId) VALUES ('f407e9c5-6c9b-4aa3-d659-08d970a67374','Sample1','C:\filestore\156855e3-fc30-492e-9d04-d7b39ba274cc.jpg','db211e24-51d1-4b4d-aed9-d00d30540b05');
INSERT INTO dbo.[File](Id,DisplayName,FileName,FolderId) VALUES ('b4ff980b-d45d-4a9d-d65a-08d970a67374','Sample1','C:\filestore\95757064-a260-4f3e-a0f9-e4b5c8edcded.jpg','59722347-d091-46e7-8aab-9664142f6b93');
INSERT INTO dbo.[File](Id,DisplayName,FileName,FolderId) VALUES ('9613f100-fa16-4a0a-d65b-08d970a67374','Sample1','C:\filestore\03132ba0-6dad-4866-bc01-f7ae11cea507.jpg','59722347-d091-46e7-8aab-9664142f6b93');
INSERT INTO dbo.[File](Id,DisplayName,FileName,FolderId) VALUES ('fa6b3965-7e16-497f-d65c-08d970a67374','Sample1','C:\filestore\a04c17ec-6b03-44db-8d80-4497a60ab200.jpg','59722347-d091-46e7-8aab-9664142f6b93');
INSERT INTO dbo.[File](Id,DisplayName,FileName,FolderId) VALUES ('e1f57981-bf99-440a-d65d-08d970a67374','Sample1','C:\filestore\491fa7ef-1e28-4577-ad27-04ccaf583766.jpg','f2c59e50-bbb6-4396-9fe8-b9156d342362');
GO