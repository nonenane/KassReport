USE [KassRealiz]
GO
/****** Object:  StoredProcedure [CashReport].[GetCashData]    Script Date: 28.10.2020 13:31:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:  <SPorykhin G Y>
-- Create date: <30.10.2020>
-- Description: <Получение данных по продаже акционных товаров>
-- =============================================
CREATE PROCEDURE [CashReport].[GetDataActionGoodsRealiz] 
	@DateStart date, 
	@DateEnd date
AS
BEGIN
 SET NOCOUNT ON;

 
	DECLARE @date_start datetime,@date_end datetime
	
		SET @date_start = @DateStart
		SET @date_end = @DateEnd

		set @date_start = DATEADD(hour,6,@date_start)
		set @date_end = DATEADD(hour,27,@date_end)


 --Подстановка нужного журнала касс под выборку
 declare @MONTH varchar(2)
 set @MONTH=(case when MONTH(@date_start)<10 then '0'+cast(MONTH(@date_start) as varchar(1)) else cast(MONTH(@date_start) as varchar(2)) end)
 declare @SQL nvarchar(MAX)
 declare @journal nvarchar(255)
 set @journal='[KassRealiz].[dbo].[journal_'+cast(YEAR(@date_start) as varchar)+'_'+@MONTH+']'  


 set @SQL = 
 '
 select
   j.terminal,
   j.doc_id into #used_docs
 from
   ' + @journal + ' j
 where
  convert(datetime, j.time) >= convert(datetime, ''' + convert(varchar,@date_start,120) + ''') and convert(datetime, j.time) <= convert(datetime, ''' + convert(varchar,@date_end,120) + ''') and op_code = 509


  select
   ltrim(rtrim(j.ean)) as ean,
   SUM(case when j.op_code = 505 then 1.0 else -1.0 end * j.count/1000.0) as count,
   j.price/100.0 as price
   INTO #resultTable
 from	
   ' + @journal + ' j
	inner join #used_docs u on u.terminal = j.terminal and u.doc_id = j.doc_id
 where
  convert(datetime, j.time) >= convert(datetime, ''' + convert(varchar,@date_start,120) + ''') and convert(datetime, j.time) <= convert(datetime, ''' + convert(varchar,@date_end,120) + ''') and op_code in (505,507)
  group by j.ean,j.price
  order by j.ean asc


select 
	nn.id_tovar,nn.cname,nn.ntypetovar 
	into #ntovar
from 
	dbase1.dbo.s_ntovar nn 
	inner join (select id_tovar,max(tdate_n) as tdate_n from dbase1.dbo.s_ntovar 
	where tdate_n<=''' + convert(varchar,@DateEnd,120) + ''' group by id_tovar) n on n.id_tovar = nn.id_tovar and n.tdate_n = nn.tdate_n


select distinct
	ltrim(rtrim(t.ean)) as ean,
	ltrim(rtrim(isnull(tt.cName,'''')))+ltrim(rtrim(isnull(n.cname,''Товар не найден''))) as cName,
	r.count,
	r.price,
	r.count * r.price as SumResult	
from 
	dbase1.requests.s_CatalogPromotionalTovars c
		inner join dbase1.dbo.s_tovar t on t.id = c.id_tovar
		left join #ntovar n on n.id_tovar = t.id
		left join dbase1.dbo.s_TypeTovar tt on tt.id = n.ntypetovar
		left join #resultTable r on ltrim(rtrim(r.ean)) = ltrim(rtrim(t.ean)) COLLATE Cyrillic_General_CS_AS
where 
	r.count is not null and r.price is not null

DROP TABLE  #used_docs, #ntovar,#resultTable
'

--PRINT @SQL
EXEC (@SQL)

END