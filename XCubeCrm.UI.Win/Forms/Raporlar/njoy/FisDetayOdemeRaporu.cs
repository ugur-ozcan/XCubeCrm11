using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using XCubeCrm.Common.Enums;
using XCubeCrm.UI.Win.Forms.BaseReport;
using XCubeCrm.UI.Win.GenelForms;

namespace XCubeCrm.UI.Win.Forms.Raporlar.NJOY
{
    public partial class FisDetayOdemeRaporu : BaseReportListForm 
    {
        sqlBaglanti bgl = new sqlBaglanti();
        string araSorgu = "";
        public FisDetayOdemeRaporu( )
        {
 
            InitializeComponent();
        }
        protected override void Listele()
        {
            araSorgu = "";
             
             araSorgu = "";
            if (AnaForm.aktifKayilar != "")
            {
                araSorgu += "AND CLC.ACTIVE IN (" + AnaForm.aktifKayilar + ")";
            }
            if (AnaForm.cariLogicalref != "")
            {
                araSorgu += "AND CLC.LOGICALREF IN (" + AnaForm.cariLogicalref + ")";
            }
            if (AnaForm.ozelKod != "")
            {
                araSorgu += "AND CLC.SPECODE IN (" + AnaForm.ozelKod + ")";
            }
            if (AnaForm.ozelKod2 != "")
            {
                araSorgu += "AND CLC.SPECODE2 IN (" + AnaForm.ozelKod2 + ")";
            }
            if (AnaForm.ozelKod3 != "")
            {
                araSorgu += "AND CLC.SPECODE3 IN (" + AnaForm.ozelKod3 + ")";
            }
            if (AnaForm.ozelKod4 != "")
            {
                araSorgu += "AND CLC.SPECODE4 IN (" + AnaForm.ozelKod4 + ")";
            }
            if (AnaForm.ozelKod5 != "")
            {
                araSorgu += "AND CLC.SPECODE5 IN (" + AnaForm.ozelKod5 + ")";
            }
            if (AnaForm.yetkiKodu != "")
            {
                araSorgu += "AND CLC.CYPHCODE IN (" + AnaForm.yetkiKodu + ")";
            }
            if (AnaForm.dovizTuru != "")
            {
                araSorgu += "AND STL.TRCURR IN (" + AnaForm.dovizTuru + ")";
            }
            if (AnaForm.satisElemani != "")
            {
                araSorgu += " and CLC.LOGICALREF IN (" + " SELECT   LOGICALREF FROM ( " +
" SELECT LOGICALREF,   " +
" (SELECT TOP 1 SL.SALESMANREF FROM  " + AnaForm.db +  ".DBO.LG_" + AnaForm.firma +  "_SLSCLREL SL  " +
" JOIN " + AnaForm.db +  ".DBO.LG_SLSMAN  SLS ON SL.CLIENTREF=CL.LOGICALREF AND SL.SALESMANREF = SLS.LOGICALREF AND SLS.FIRMNR =" + AnaForm.firma +  " ORDER BY  SL.LOGICALREF DESC ) AS SALESMANREF " +
" FROM " + AnaForm.db +  ".DBO.LG_" + AnaForm.firma +  "_CLCARD CL " +
" ) AS AAA WHERE AAA.SALESMANREF IN ( " + AnaForm.satisElemani + "))";
            }
            if (AnaForm.aktifKayitlarUrun != "")
            {
                araSorgu += "AND ITS.ACTIVE IN (" + AnaForm.aktifKayitlarUrun + ")";
            }
            if (AnaForm.urunLogicalref!="")
            {
                araSorgu += "AND ITS.LOGICALREF IN (" + AnaForm.urunLogicalref + ")";
            }
            if (AnaForm.urunOzelKod != "")
            {
                araSorgu += "AND ITS.SPECODE IN (" + AnaForm.urunOzelKod + ")";
            }
            if (AnaForm.urunOzelKod2 != "")
            {
                araSorgu += "AND ITS.SPECODE2 IN (" + AnaForm.urunOzelKod2 + ")";
            }
            if (AnaForm.urunOzelKod3 != "")
            {
                araSorgu += "AND ITS.SPECODE3 IN (" + AnaForm.urunOzelKod3 + ")";
            }
            if (AnaForm.urunOzelKod4 != "")
            {
                araSorgu += "AND ITS.SPECODE4 IN (" + AnaForm.urunOzelKod4 + ")";
            }
            if (AnaForm.urunOzelKod5 != "")
            {
                araSorgu += "AND ITS.SPECODE5 IN (" + AnaForm.urunOzelKod5 + ")";
            }
            if (AnaForm.urunGrup != "")
            {
                araSorgu += "AND ITS.STGRPCODE IN (" + AnaForm.urunGrup + ")";
            }
            if (AnaForm.urunTip != "")
            {
                araSorgu += "AND ITS.CARDTYPE IN (" + AnaForm.urunTip + ")";
            }
            if (AnaForm.ambar != "")
            {
                araSorgu += " AND  L_CAPIDIV.NR IN (" + AnaForm.isyeri + ")";
            }


            string sorgu3 = @"DECLARE @DynamicPivotColumns NVARCHAR(MAX)
DECLARE @DynamicPivotCariColumns NVARCHAR(MAX)
DECLARE @DynamicQuery NVARCHAR(MAX)
/* Banka POS sütunlarını dinamik olarak oluştur */
SELECT @DynamicPivotColumns = STRING_AGG(QUOTENAME(AD), ',')
FROM BANKA_POS
/* Cari tahsilat sütunlarını dinamik olarak oluştur */
SELECT @DynamicPivotCariColumns = STRING_AGG(QUOTENAME(AD), ',')
FROM (
    SELECT DISTINCT C.AD 
    FROM FINANS_DETAY D 
    JOIN CARI C ON C.ID = D.KART_BORCLU
    WHERE D.FINANS_ISLEM_TURU = 68
) AS CariList
SET @DynamicQuery = N'
WITH Fisler As (
    SELECT F.* ,T.IADE,T.AD AS FIS_TURU_AD
    FROM FIS F
    INNER JOIN FIS_TURU T ON T.ID=F.FIS_TURU
    WHERE F.ID>0 
    AND F.FIS_TARIHI BETWEEN ''2024-01-20 23:59:59'' AND ''2024-12-20 23:59:59''
    AND F.LOKASYON IN (438206)
    AND (F.FIS_TURU IN (11,12,35,36))   		
), 
PosOdemeler AS (
    SELECT 
        D.FIS,
        BP.AD AS BANKA_POS_ADI,
        ABS(D.TUTAR*IIF(D.FINANS_ISLEM_TURU=55,-1,1)) AS TUTAR
    FROM FINANS_DETAY D
    JOIN BANKA_POS BP ON BP.ID = D.KART_BORCLU
    WHERE D.FINANS_ISLEM_TURU IN (15,55) 
    AND D.KART_ALACAKLI > 0
),
CariOdemeler AS (
    SELECT 
        D.FIS,
        C.AD AS CARI_ADI,
        ABS(D.TUTAR) AS TUTAR
    FROM FINANS_DETAY D
    JOIN CARI C ON C.ID = D.KART_BORCLU
    WHERE D.FINANS_ISLEM_TURU = 68
),
PosPivot AS (
    SELECT *
    FROM PosOdemeler
    PIVOT (
        SUM(TUTAR)
        FOR BANKA_POS_ADI IN (' + @DynamicPivotColumns + ')
    ) AS PVT
),
CariPivot AS (
    SELECT *
    FROM CariOdemeler
    PIVOT (
        SUM(TUTAR)
        FOR CARI_ADI IN (' + @DynamicPivotCariColumns + ')
    ) AS PVT
),
Tahsilat As (
    SELECT 
        T.FIS,
        T.GENELTOPLAM,
        T.GENELTOPLAM*IIF(T.IADE=1,-1,1) TUTAR,
        SUM(T.NAKIT)*IIF(T.IADE=1,-1,1) NAKIT,
        ABS(SUM(T.KREDIKARTI))*IIF(T.IADE=1,-1,1) KREDI_KARTI,
        ABS(SUM(T.CARI_TAHSILAT))*IIF(T.IADE=1,-1,1) CARI_TAHSILAT,
        ABS(SUM(T.PUAN))*IIF(T.IADE=1,-1,1) PUAN,
        ABS(SUM(T.KUPON))*IIF(T.IADE=1,-1,1) KUPON,
        ABS(SUM(T.NAKIT+T.KREDIKARTI+T.CARI_TAHSILAT+T.PUAN+T.KUPON))*IIF(T.IADE=1,-1,1) TAHSILAT
    FROM (
        SELECT 
            D.FIS,
            F.GENELTOPLAM,
            F.IADE,
            CASE WHEN D.FINANS_ISLEM_TURU IN (1,2) THEN D.TUTAR*IIF(D.FINANS_ISLEM_TURU=2,-1,1) ELSE 0 END NAKIT,
            CASE WHEN D.FINANS_ISLEM_TURU IN (15,55) AND D.KART_ALACAKLI>0 THEN D.TUTAR*IIF(D.FINANS_ISLEM_TURU=55,-1,1) ELSE 0 END KREDIKARTI,
            CASE WHEN D.FINANS_ISLEM_TURU IN (68) THEN D.TUTAR ELSE 0 END CARI_TAHSILAT,
            CASE WHEN D.FINANS_ISLEM_TURU IN (79) THEN D.TUTAR ELSE 0 END PUAN,
            CASE WHEN D.FINANS_ISLEM_TURU IN (119) THEN D.TUTAR ELSE 0 END KUPON
        FROM FINANS_DETAY D
        JOIN FINANS_ISLEM_TURU T ON T.ID=D.FINANS_ISLEM_TURU
        JOIN Fisler F ON F.ID=D.FIS
        WHERE D.FINANS_ISLEM_TURU NOT IN (53,54,100,104)
    ) AS T
    GROUP BY T.FIS,T.GENELTOPLAM,T.IADE
)
SELECT 
    L.FIS,
    L.FIS_TURU,
    L.BELGENO,
    L.FIS_TARIHI,
    L.LOKASYON,
    L.MIKTAR_FIS,
    L.FIYAT,
    L.AD,
    L.DAHIL_FIYAT,
    L.TUTAR,
    L.DAHIL_TUTAR,
    L.SATIR_YUVARLAMA,
    L.NET_KDV_DAHIL_TUTAR AS SATIR___DAHIL_TUTAR___NET,
    L.TOPLAM_KDV,
    CASE WHEN L.SIRA=1 THEN IIF(L.IADE=1,-1,1)*L.FIS_ISKONTO_TOPLAM ELSE 0 END FIS_ISKONTO_TOPLAM,
    CASE WHEN L.SIRA=1 THEN L.GENELTOPLAM ELSE 0 END GENELTOPLAM,
    CASE WHEN L.SIRA=1 THEN IIF(L.IADE=1,-1,1)*L.FIS_KDV_TOPLAM ELSE 0 END FIS_KDV_TOPLAM,
    L.SIRA,
    T.NAKIT,
    T.KREDI_KARTI,
    T.CARI_TAHSILAT AS TAHSILAT_CARISI,
    T.PUAN,
    T.KUPON,
    T.TAHSILAT,
	    CASE WHEN L.SIRA=1 THEN COALESCE(L.GENELTOPLAM,0)-COALESCE(T.TAHSILAT,0) ELSE 0 END VERESIYE, 
    P.*,
    C.*
	FROM (
    SELECT 
        D.FIS,
        F.FIS_TURU_AD AS FIS_TURU,
        F.BELGENO,
        F.FIS_TARIHI,
        L.AD LOKASYON,
        D.MIKTAR_FIS,
        D.FIYAT,
        S.AD,
        D.DAHIL_FIYAT,
        D.TUTAR,
        D.DAHIL_TUTAR,
        ((F.YUVARLAMA*DBO.BOL(SUM(D.TUTAR*((100-D.ISKONTO_HESAP)/100)) OVER (PARTITION BY D.FIS),
          SUM(D.TUTAR*((100-D.ISKONTO_HESAP)/100)*((100+D.KDV_TOPTAN)/100)) OVER (PARTITION BY D.FIS))))*
         DBO.BOL((D.TUTAR*((100-D.ISKONTO_HESAP)/100)), 
         SUM(D.TUTAR*((100-D.ISKONTO_HESAP)/100)) OVER (PARTITION BY D.FIS)) SATIR_YUVARLAMA,
        ((D.TUTAR*((100-D.ISKONTO_HESAP)/100))-
         (((F.YUVARLAMA*DBO.BOL(SUM(D.TUTAR*((100-D.ISKONTO_HESAP)/100)) OVER (PARTITION BY D.FIS),
           SUM(D.TUTAR*((100-D.ISKONTO_HESAP)/100)*((100+D.KDV_TOPTAN)/100)) OVER (PARTITION BY D.FIS))))*
          DBO.BOL((D.TUTAR*((100-D.ISKONTO_HESAP)/100)), 
          SUM(D.TUTAR*((100-D.ISKONTO_HESAP)/100)) OVER (PARTITION BY D.FIS))))*
         ((D.KDV_TOPTAN+100)/100)*IIF(F.IADE=1,-1,1) NET_KDV_DAHIL_TUTAR,
        F.FIS_ISKONTO_TOPLAM,
        D.TOPLAM_KDV,
        (IIF(F.IADE=1,-1,1)*F.GENELTOPLAM) AS GENELTOPLAM,
        F.KDV_TOPLAM FIS_KDV_TOPLAM,
        F.IADE,
        ROW_NUMBER() OVER (PARTITION BY D.FIS ORDER BY D.ID) SIRA
    FROM FIS_DETAY D
    INNER JOIN Fisler F ON F.ID=D.FIS
    LEFT JOIN CARI C ON C.ID=F.CARI
    LEFT JOIN LOKASYON L ON L.ID=D.LOKASYON
    LEFT JOIN STOK S ON S.ID=D.STOK
) AS L
LEFT JOIN Tahsilat T ON T.FIS=L.FIS AND L.SIRA=1
LEFT JOIN PosPivot P ON P.FIS = L.FIS
LEFT JOIN CariPivot C ON C.FIS = L.FIS
'
EXEC sp_executesql @DynamicQuery ";




            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sorgu3, bgl.baglantiNJOY());

            
            da.SelectCommand.CommandTimeout = 0;
            da.Fill(dt1);
            grid.DataSource = dt1;

           // AnaForm frm = new AnaForm();
          
             

           
        }

         
        protected override void DegiskenleriDoldur()
        {
            Tablo = tablo;
              
            BaseKartTuru = KartTuru.FisDetayOdemeRaporu;
        }

          
    }
}