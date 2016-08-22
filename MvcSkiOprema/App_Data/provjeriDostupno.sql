CREATE TRIGGER provjeriDostupno
ON dbo.Oprema
FOR INSERT, UPDATE
AS
BEGIN
	DECLARE @dostupno INT;
	SET @dostupno = (select dostupno from inserted);
    IF @dostupno < 0 
	BEGIN
		RAISERROR ('Greska vrijednosti atributa dostupno', 
               19, -- Severity level
               -1 -- State
               );
	END 
END