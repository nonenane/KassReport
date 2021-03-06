USE [KassRealiz]
GO
/****** Object:  StoredProcedure [CashReport].[GetCashData]    Script Date: 28.10.2020 13:31:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:  <Korshkova N.>
-- Create date: <30.08.2016>
-- Description: <Получение данных с касс за период.>

-- Author:  <SAA>
-- Create date: <29.10.2019>
-- Update description: <Добавлено юридическое лицо>

-- UPDATE:	KAV 2020-05-20 подгружается отдел в #goods
-- =============================================
ALTER PROCEDURE [CashReport].[GetCashData] --'2020-01-12', '2020-01-14'
	@date_start datetime,
    @date_end datetime, 
    @id_otdel int = null,
    @terminal int = null, 
    @user_id int = null, 
    @doc_start int = null, 
    @doc_end int = null, 
    @sum_start int = null, 
    @sum_end int = null,
    @count_start int = null,
    @count_end int = null
AS
BEGIN
 SET NOCOUNT ON;

 --Подстановка нужного журнала касс под выборку
 declare @MONTH varchar(2)
 set @MONTH=(case when MONTH(@date_start)<10 then '0'+cast(MONTH(@date_start) as varchar(1)) else cast(MONTH(@date_start) as varchar(2)) end)
 declare @SQL varchar(MAX)
 declare @journal varchar(255)
 set @journal='[KassRealiz].[dbo].[journal_'+cast(YEAR(@date_start) as varchar)+'_'+@MONTH+']'   
 

 --Нахождение ЮЛ ТК
 select s_tovar.ean, 
 (case when abr1. Abbriviation is null then (case when abr2.Abbriviation is null then (abr3.Abbriviation COLLATE Cyrillic_General_CS_AS)
											 else (abr2.Abbriviation COLLATE Cyrillic_General_CS_AS) end)
 else (abr1.Abbriviation COLLATE Cyrillic_General_CS_AS) end) as abr
  into #tableLegalEntityTK
  from dbase1.dbo.s_tovar
  LEFT JOIN 
	(select distinct s_tovar.id, goods_vs_firms.ntypeorg, s_MainOrg.Abbriviation, maxDate.maxDate 
	from dbase1.dbo.s_tovar 
	JOIN dbase1.dbo.goods_vs_firms on s_tovar.id = goods_vs_firms.id_tovar
	JOIN dbase1.dbo.firms_vs_departments on firms_vs_departments.id_departments = s_tovar.id_otdel
	JOIN dbo.s_MainOrg on s_MainOrg.nTypeOrg = goods_vs_firms.ntypeorg
	JOIN (select id_tovar, date as maxDate 
		   from dbase1.dbo.goods_vs_firms where date =
									(select top 1 gf.date from dbase1.dbo.goods_vs_firms gf 
										where gf.id_tovar = goods_vs_firms.id_tovar 
										and gf.send = 1
										and gf.date <= @date_end 
	order by gf.date desc)) as maxDate on maxDate.id_tovar = goods_vs_firms.id_tovar and maxDate.maxDate = goods_vs_firms.date
	where s_MainOrg.isSeler = 1
	and s_MainOrg.DateStart <= @date_start
	and s_MainOrg.DateEnd >= @date_end
	and firms_vs_departments.DateStart <= @date_start
	and firms_vs_departments.DateEnd >= @date_end
	and goods_vs_firms.ntypeorg is not null
	) abr1 on abr1.id = s_tovar.id

LEFT JOIN (
	select distinct s_tovar.id, s_MainOrg.Abbriviation from dbase1.dbo.s_tovar
	JOIN dbase1.dbo.s_grp1 on s_grp1.id_otdel = s_tovar.id_otdel
	JOIN dbo.s_MainOrg on s_MainOrg.nTypeOrg = s_grp1.ntypeorg
	JOIN dbase1.dbo.firms_vs_departments on firms_vs_departments.id_departments = s_tovar.id_otdel --and firms_vs_departments.ntypeorg = s_MainOrg.nTypeOrg 
	where s_MainOrg.isSeler = 1
	and firms_vs_departments.[default] = 1
	and firms_vs_departments.DateStart <= @date_start
	and firms_vs_departments.DateEnd >= @date_end
	and s_MainOrg.DateStart <=  @date_start 
	and s_MainOrg.DateEnd >= @date_end) abr2 on abr2.id = s_tovar.id

LEFT JOIN
	(select distinct v_tovar.id_tovar, s_MainOrg.Abbriviation FROM dbase1.dbo.firms_vs_departments 
	JOIN dbo.s_MainOrg on s_MainOrg.nTypeOrg = firms_vs_departments.ntypeorg
	JOIN dbase1.dbo.v_tovar on v_tovar.id_otdel = firms_vs_departments.id_departments
	JOIN dbase1.dbo.departments on departments.id = v_tovar.id_otdel
	where  firms_vs_departments."default" = 1
	and firms_vs_departments.DateStart <=  @date_start
	and firms_vs_departments.DateEnd >= @date_end
	and s_MainOrg.isSeler = 1
	and s_MainOrg.DateStart <=  @date_start
	and s_MainOrg.DateEnd >= @date_end) abr3 on abr3.id_tovar = s_tovar.id
	order by s_tovar.id asc




 -- условие where
 --convert(date, j.time) = convert(date, ''' + convert(varchar,@date_start,120) + ''') and op_code = 512
 -- j.time between '+ convert(varchar,@date_start, 120) +' and '+ convert(varchar,@date_end,120) +' and op_code = 512
 set @SQL = 
 '
 select
   j.terminal,
   j.doc_id into #annul_docs
 from
   ' + @journal + ' j
 where
  convert(datetime, j.time) >= convert(datetime, ''' + convert(varchar,@date_start,120) + ''') and convert(datetime, j.time) <= convert(datetime, ''' + convert(varchar,@date_end,120) + ''') and op_code = 512
         
 select
   j.terminal,
   j.[rec_id],
   rtrim(ltrim(u.FIO)) as kassir_name,
   j.doc_id,
   convert(time, j.time) as time,
   rtrim(ltrim(op.description)) as operation_name,
   j.ean,
   convert(numeric(13,3), convert(numeric(13,3), j.count)/1000) as count,
   convert(numeric(13,2), convert(numeric(13,2), j.cash_val)/100) as cash_val,
   j.op_code,
   j.id_post,
   org.Abbriviation as legalEntity,
   case when ann.terminal is null then 0 else 1 end as is_annul,
   j.dpt_no as idDep,
   j.price/100.0 as price
   INTO #goods
 from
   ' + @journal + ' j left join dbase1.sendFrontol.s_Users u on j.user_id = (u.id COLLATE Cyrillic_General_CS_AS)
          left join dbo.operations op on j.op_code = op.op_code
          left join #annul_docs ann on j.terminal = ann.terminal and j.doc_id = ann.doc_id
		  left join dbo.s_MainOrg org on org.id_Post = j.id_post and j.ean not like ''''

 where 	
 j.time >= ''' + convert(varchar,@date_start,120) + ''' and j.time <= ''' + convert(varchar,@date_end,120) + '''
   ' + case when @id_otdel is null or @id_otdel = 0 then '' else ' and j.dpt_no = ' + rtrim(ltrim(STR(@id_otdel))) end + '
   ' + case when @terminal is null or @terminal = 0 then '' else ' and j.terminal = ' + rtrim(ltrim(STR(@terminal))) end + '
   ' + case when @user_id is null or @user_id = 0 then '' else ' and j.user_id = ' + rtrim(ltrim(STR(@user_id))) end + '
   ' + case when @doc_start is null then '' else ' and j.doc_id >= ' + rtrim(ltrim(STR(@doc_start))) end + '
   ' + case when @doc_end is null then '' else ' and j.doc_id <= ' + rtrim(ltrim(STR(@doc_end))) end + '
   ' + case when @sum_start is null then '' else ' and j.cash_val >= ' + rtrim(ltrim(STR(@sum_start*100))) end + '
   ' + case when @sum_end is null then '' else ' and j.cash_val <= ' + rtrim(ltrim(STR(@sum_end*100))) end + '
   ' + case when @count_start is null then '' else ' and len(ean) = 4 and j.count >= ' + rtrim(ltrim(str(@count_start))) end + '
   ' + case when @count_end is null then '' else ' and len(ean) = 4 and j.count <= ' + rtrim(ltrim(str(@count_end))) end + '

 order by
   j.terminal, j.doc_id, j.time 
   
 select --distinct
   g.*,
   rtrim(ltrim(isnull(gr_isi.cname,''''))) as group_name,
   rtrim(ltrim(isnull(t_isi.cname,''''))) as cname,
   min( leTK.abr) as legalEntityTK,
   cpt.id as id_promo
 from
   #goods g 
      left join dbase1.dbo.v_tovar t_isi on rtrim(ltrim(g.ean)) = rtrim(ltrim(t_isi.ean)) collate Cyrillic_General_CS_AS
      left join dbase1.dbo.s_grp1 gr_isi on t_isi.id_grp1 = gr_isi.id
	  left join #tableLegalEntityTK leTK on rtrim(ltrim(leTK.ean)) = rtrim(ltrim(g.ean)) collate Cyrillic_General_CS_AS
	  left join dbase1.requests.s_CatalogPromotionalTovars cpt on cpt.id_tovar = t_isi.id_tovar
group by  g.[rec_id],g.terminal, g.kassir_name, g.doc_id, g.time,  g.operation_name, g.ean,
			 g.count , g.cash_val , g.op_code , g.id_post , g.legalEntity , g.is_annul , g.idDep
			 ,gr_isi.cname,g.price, t_isi.cname, cpt.id --, leTK.abr
order by g.[rec_id]

 
 
 
 
 
 drop table #annul_docs
 drop table #goods
 drop table #tableLegalEntityTK
 '
 
 print @SQL
 exec (@SQL)
END