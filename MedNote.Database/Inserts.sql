INSERT INTO [dbo].[SEG_CLIENTE]([TX_NOME] ,[TX_DOCUMENTO],[TX_SIGLA] ,[TX_IDENTIFICADOR] ,[DT_CADASTRO],[DT_DESATIVACAO],[BL_DESATIVO])
     VALUES ('unidade 1', '99999999000191','UND',null,getdate(),null,0)
GO

INSERT INTO [dbo].[SEG_ESPECIALIDADE] ([CD_CLIENTE],[TX_DESCRICAO] ,[BL_MEDICA],[DT_CADASTRO],[DT_DESATIVACAO],[BL_DESATIVADO])
     VALUES ((select cd_cliente from SEG_CLIENTE where TX_SIGLA = 'UND'),'Especialidade Padrão',1,getdate() ,null,0)
GO

INSERT INTO [dbo].[SEG_PERFIL]([TX_DESCRICAO],[DT_CADASTRO],[DT_DESATIVACAO],[BL_DESATIVADO])
     VALUES ('Administrador',getdate(),null,1),
			('Medico',getdate(),null,1)
GO


INSERT INTO [dbo].[SEG_USUARIO] ([CD_CLIENTE],[CD_ESPECIALIDADE] ,[CD_PERFIL] ,[TX_NOME],[TX_DOCUMENTO],[TX_EMAIL],[TX_LOGIN] ,[TX_SENHA],[DT_CADASTRO] ,[DT_DESATIVACAO],[BL_DESATIVACAO])
     VALUES ((select cd_cliente from SEG_CLIENTE where TX_SIGLA = 'UND'),
			 (select cd_ESPECIALIDADE from SEG_ESPECIALIDADE where TX_DESCRICAO = 'Especialidade Padrão'),
			 (select cd_PERFIL from SEG_PERFIL where TX_DESCRICAO = 'Administrador'),
			  'ADM',
			  '13857633700',
			  'juliana.hote@globalsys.com.br',
			  '13856733700',
			  'teste',
			   getdate(),
               null,
               1)
GO


delete from SEG_FUNCOES
go
delete from SEG_ACAO
go
--Alterar esse script quando a estrutura do sistema for definitiva
INSERT INTO [SEG_FUNCOES]
           ([TX_NOME],[TX_DESCRICAO],[DT_DESATIVACAO],[TX_REF],[TX_TIPO],[TX_COR],[TX_ICON],[TX_CANAL],[CD_PAI],[DT_CADASTRO])
    VALUES
			('Segurança', 'Segurança', NULL, null, 'Modulo', '#FFFFFF', 'fa fa-lock', 'Ambos', null, getdate()),
			('Sistema', 'Sistema', NULL, null, 'Modulo', '#FFFFFF', 'fa fa-lock', 'Ambos', null, getdate()),
			('MedNote', 'MedNote', NULL, null, 'Modulo', '#FFFFFF', 'fa fa-lock', 'Ambos', null, getdate()),
			('Cliente', 'Cliente', NULL, 'app.seguranca-cliente', 'Pagina', '#FFFFFF', '#', 'Ambos', (select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Sistema'), getdate()),
			('Especialidade', 'Especialidades', NULL, 'app.seguranca-especialidade', 'Pagina', '#FFFFFF', '#', 'Ambos', (select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Sistema'), getdate()),
			('Usuario', 'Usuario', NULL, 'app.seguranca-usuario', 'Pagina', '#FFFFFF', '#', 'Ambos', (select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Segurança'), getdate()),
			('Perfil', 'Perfil', NULL, 'app.seguranca-grupo', 'Pagina', '#FFFFFF', '#', 'Ambos', (select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Segurança'), getdate()),
			('Permissoes', 'Permissões de Grupos', NULL, 'app.seguranca-permissoes-grupos', 'Pagina', '#FFFFFF', '#', 'Ambos', (select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Segurança'), getdate()),
			('PermissoesUsuarios', 'Permissões de Usuários', NULL, 'app.seguranca-permissoes-usuarios', 'Pagina', '#FFFFFF', '#', 'Ambos', (select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Segurança'), getdate()),
			('TipoSetor', 'Tipo de Setor', NULL, 'app.mednote-tiposetor', 'Pagina', '#FFFFFF', '#', 'Ambos', (select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Sistema'), getdate()),
			('Setor', 'Setor', NULL, 'app.mednote-setor', 'Pagina', '#FFFFFF', '#', 'Ambos', (select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Sistema'), getdate()),
			('Admissao', 'Admissão', NULL, 'app.mednote-admissao', 'Pagina', '#FFFFFF', '#', 'Ambos', (select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'MedNote'), getdate()),
			('Internacao', 'Painel de Internações', NULL, 'app.mednote-internacao', 'Pagina', '#FFFFFF', '#', 'Ambos', (select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'MedNote'), getdate())

GO 

INSERT INTO [SEG_ACAO]
           ([TX_NOME],[DT_DESATIVACAO],[TX_REF],[CD_FUNCAO],[DT_CADASTRO])
     VALUES
           ('Get', NULL,'Get',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Usuario'),GETDATE()),
           ('Get/id', NULL,'Get/id',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Usuario'),GETDATE()),
           ('Post', NULL,'Post',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Usuario'),GETDATE()),
           ('Put', NULL,'Put',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Usuario'),GETDATE()),
		   
		   ('Get', NULL,'Get',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Permissoes'),GETDATE()),
           ('Get/id', NULL,'Get/id',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Permissoes'),GETDATE()),
           ('Post', NULL,'Post',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Permissoes'),GETDATE()),
           ('Put', NULL,'Put',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Permissoes'),GETDATE()),
		   
		   ('Get', NULL,'Get',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'PermissoesUsuarios'),GETDATE()),
           ('Get/id', NULL,'Get/id',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'PermissoesUsuarios'),GETDATE()),
           ('Post', NULL,'Post',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'PermissoesUsuarios'),GETDATE()),
           ('Put', NULL,'Put',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'PermissoesUsuarios'),GETDATE()),
		   
           ('Get', NULL,'Get',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Perfil'),GETDATE()),
           ('Get/id', NULL,'Get/id',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Perfil'),GETDATE()),
           ('Post', NULL,'Post',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Perfil'),GETDATE()),
           ('Put', NULL,'Put',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Perfil'),GETDATE()),

		   ('Get', NULL,'Get',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Cliente'),GETDATE()),
           ('Get/id', NULL,'Get/id',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Cliente'),GETDATE()),
           ('Post', NULL,'Post',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Cliente'),GETDATE()),
           ('Put', NULL,'Put',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Cliente'),GETDATE()),
		   
		   ('Get', NULL,'Get',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Especialidade'),GETDATE()),
           ('Get/id', NULL,'Get/id',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Especialidade'),GETDATE()),
           ('Post', NULL,'Post',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Especialidade'),GETDATE()),
           ('Put', NULL,'Put',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Especialidade'),GETDATE()),

		   ('Get', NULL,'Get',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'TipoSetor'),GETDATE()),
           ('Get/id', NULL,'Get/id',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'TipoSetor'),GETDATE()),
           ('Post', NULL,'Post',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'TipoSetor'),GETDATE()),
           ('Put', NULL,'Put',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'TipoSetor'),GETDATE()),		   

		   ('Get', NULL,'Get',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Setor'),GETDATE()),
           ('Get/id', NULL,'Get/id',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Setor'),GETDATE()),
           ('Post', NULL,'Post',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Setor'),GETDATE()),
           ('Put', NULL,'Put',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Setor'),GETDATE()),	
		   
		   ('Get', NULL,'Get',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Admissao'),GETDATE()),
           ('Get/id', NULL,'Get/id',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Admissao'),GETDATE()),
           ('Post', NULL,'Post',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Admissao'),GETDATE()),
           ('Put', NULL,'Put',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Admissao'),GETDATE()),

		   ('Get', NULL,'Get',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Internacao'),GETDATE()),
           ('Get/id', NULL,'Get/id',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Internacao'),GETDATE()),
           ('Post', NULL,'Post',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Internacao'),GETDATE()),
           ('Put', NULL,'Put',(select CD_FUNCAO from SEG_FUNCOES where TX_NOME = 'Internacao'),GETDATE())	
GO

 