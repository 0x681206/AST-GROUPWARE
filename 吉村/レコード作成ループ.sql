TRUNCATE TABLE T_BUKKENCOMMENT;

DECLARE
	@recordCount INT=10000,
	@i INT,
	@bukken_cd INT=1

SET @i = 1

--レコード数の数だけ繰り返し処理を実施
WHILE @i <=@recordCount
　　BEGIN
　　　　insert into T_BUKKEN (bukken_cd,bukken_name,update_user,update_date) values(@bukken_cd,@bukken_cd,1,'2023-11-1');
		SET @i = @i +1
		if @i=10000
			begin
				set @bukken_cd =@bukken_cd+1;
			end
　　END
--レコード数の数だけ繰り返し処理を実施
WHILE @i <=@recordCount
　　BEGIN
　　　　insert into T_BUKKENCOMMENT (bukken_cd,comment_cd,message,update_user,update_date) values(@bukken_cd,@i+100,'system',1,'2023-11-1');
		SET @i = @i +1
		if @i=10000
			begin
				set @bukken_cd =@bukken_cd+1;
			end
　　END