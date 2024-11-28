using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XCubeCrm.UI.Win.Forms.BaseReport;
using XCubeCrm.UI.Win.GenelForms;

namespace XCubeCrm.UI.Win.Forms.Raporlar.NJOY
{
    public partial class ZRaporu : BaseReportListForm
    {
        sqlBaglanti bgl = new sqlBaglanti();
        public ZRaporu( )
        {
           
            InitializeComponent();
        }
        protected override void Listele()
        {

            //            string sorgu3 = @"
            //WITH TAHSILAT_TIPLERI AS (
            //    -- Nakit (Grup 1)
            //    SELECT 'Nakit' AS AD, 1 AS GRUP_SIRA
            //    UNION
            //    -- Banka POS Tahsilatları (Grup 2)
            //    SELECT BP.AD, 2 AS GRUP_SIRA
            //    FROM BANKA_POS BP
            //    JOIN BANKA B ON B.ID = BP.BANKA
            //    WHERE BP.AKTIF = 1 
            //    AND B.AKTIF = 1
            //    UNION
            //    -- Müşteri Tahsilatları (Grup 3)
            //    SELECT C.AD, 3 AS GRUP_SIRA
            //    FROM CARI C 
            //    WHERE C.AKTIF = 1 
            //    AND C.TAHSILAT_CARISI = 1
            //    UNION
            //    -- Açık Hesap (Grup 4)
            //    SELECT 'Açık hesap', 4 AS GRUP_SIRA
            //),
            //SATIS_TUTARLARI AS (
            //    SELECT 
            //        TT.AD,
            //        TT.GRUP_SIRA,
            //        COALESCE(DOVIZ_AD.AD, 'TRY') AS DOVIZ_AD,
            //        CAST(COALESCE(SUM(POS_TAHSILAT_W_FIS.TUTAR), 0) AS DECIMAL(18,2)) AS SATIS_TUTAR,
            //        CAST(COALESCE(SUM(POS_TAHSILAT_W_FIS.TUTAR) * COALESCE(DOVIZ_KUR_SON.SATIS, 1), 0) AS DECIMAL(18,2)) AS SATIS_TUTAR_TL
            //    FROM TAHSILAT_TIPLERI TT
            //    LEFT JOIN POS_TAHSILAT_W_FIS ON TT.AD = POS_TAHSILAT_W_FIS.AD
            //    LEFT JOIN POS_GECICI ON POS_GECICI.ID = POS_TAHSILAT_W_FIS.POS_GECICI 
            //        AND POS_GECICI.KAPANDI = 1
            //        AND POS_GECICI.LOKASYON IN (" + lokasyonIDs + @")
            //        AND POS_GECICI.TARIH BETWEEN @BaslangicTarihi AND @BitisTarihi
            //    LEFT JOIN DOVIZ_AD ON POS_TAHSILAT_W_FIS.DOVIZ_AD = DOVIZ_AD.ID
            //    LEFT JOIN DOVIZ_KUR_SON_V AS DOVIZ_KUR_SON ON DOVIZ_KUR_SON.DOVIZ_AD = POS_TAHSILAT_W_FIS.DOVIZ_AD
            //    GROUP BY TT.AD, TT.GRUP_SIRA, DOVIZ_AD.AD, DOVIZ_KUR_SON.SATIS
            //),
            //TAHSILAT_DETAY AS (
            //    SELECT 
            //        PG.ID AS POS_GECICI,
            //        FD.FINANS_ISLEM_TURU,
            //        CASE 
            //            WHEN FD.FINANS_ISLEM_TURU = 15 THEN BP.AD
            //            WHEN FD.FINANS_ISLEM_TURU = 68 THEN C.AD
            //            WHEN FD.FINANS_ISLEM_TURU IN (1,2) THEN 'Nakit'
            //            ELSE 'Açık hesap'
            //        END AS TAHSILAT_ADI,
            //        CAST(FD.TUTAR AS DECIMAL(18,2)) AS TUTAR,
            //        DV.AD AS DOVIZ_ADI,
            //        FIT.AD AS FINANS_ISLEM_TURU_ADI,
            //        COALESCE(DK.SATIS, 1) AS KUR
            //    FROM POS_GECICI PG
            //    INNER JOIN FINANS_DETAY FD ON FD.FIS = PG.ID
            //    LEFT JOIN BANKA_POS BP ON BP.ID = FD.KART_BORCLU AND FD.FINANS_ISLEM_TURU = 15
            //    LEFT JOIN CARI C ON C.ID = FD.KART_BORCLU AND FD.FINANS_ISLEM_TURU = 68
            //    LEFT JOIN DOVIZ_AD DV ON DV.ID = FD.DOVIZ_AD
            //    LEFT JOIN FINANS_ISLEM_TURU FIT ON FIT.ID = FD.FINANS_ISLEM_TURU
            //    LEFT JOIN DOVIZ_KUR_SON_V DK ON DK.DOVIZ_AD = FD.DOVIZ_AD
            //    WHERE PG.KAPANDI = 1 
            //    AND PG.LOKASYON IN (" + lokasyonIDs + @")
            //    AND PG.TARIH BETWEEN @BaslangicTarihi AND @BitisTarihi
            //)
            //SELECT 
            //    TAHSILAT_ADI,
            //    CAST(SATIS_TL AS DECIMAL(18,2)) AS SATIS_TL,
            //    CAST(TAHSILAT_TL AS DECIMAL(18,2)) AS TAHSILAT_TL,
            //    CAST((SATIS_TL + TAHSILAT_TL) AS DECIMAL(18,2)) AS TOPLAM_CIRO
            //FROM (
            //    SELECT * FROM NORMAL_ROWS
            //    UNION ALL
            //    SELECT 
            //        'TOPLAM' AS TAHSILAT_ADI,
            //        999 AS GRUP_SIRA,
            //        DOVIZ_AD,
            //        SUM(SATIS) AS SATIS,
            //        SUM(SATIS_TL) AS SATIS_TL,
            //        SUM(TAHSILAT) AS TAHSILAT,
            //        SUM(TAHSILAT_TL) AS TAHSILAT_TL
            //    FROM NORMAL_ROWS
            //    GROUP BY DOVIZ_AD
            //) AS ASD 
            //ORDER BY GRUP_SIRA, TAHSILAT_ADI";
            //        }


            string sorgu3 = @"asd";
        DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sorgu3, bgl.baglanti());


            da.SelectCommand.CommandTimeout = 0;
            da.Fill(dt1);
            grid.DataSource = dt1;



            DataTable dt2 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(sorgu3, bgl.baglanti());

            da1.SelectCommand.CommandTimeout = 0;
            da1.Fill(dt2);
            grid2.DataSource = dt2;
        }
    }
}
