CREATE DATABASE PowerPath;

USE PowerPath;

CREATE TABLE Medidor (
    Instalação VARCHAR(10) NOT NULL,
    Lote INT NOT NULL,
    Operadora VARCHAR(5) NOT NULL,
    Fabricante VARCHAR(15) NOT NULL,
    Modelo INT NOT NULL,
    Versão INT NOT NULL,
	Criacao DATETIME2 NOT NULL,
	Alteracao DATETIME2 NULL,
    Excluido BIT NOT NULL DEFAULT 0,
    PRIMARY KEY (Instalacao, Lote)
) CHARACTER SET utf8mb4 COLLATE utf8mb4_bin;