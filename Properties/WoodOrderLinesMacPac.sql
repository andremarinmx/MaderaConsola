CREATE TABLE WoodOrderLinesMacPac(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Orden VARCHAR(255),
	Line int,
	MO VARCHAR(255),
	Table96 FLOAT,
	Bar144 FLOAT,
	Bar120 FLOAT,
	Bar96 FLOAT,
	Pallet48x48 INT,
	Pallet40x48 INT,
	Pallet32X32 INT,
	Pallet34X56 INT
);