﻿//using DevExpress.Utils.Extensions;
//using DevExpress.XtraBars;
//using DevExpress.XtraEditors;
//using DevExpress.XtraGrid.Views.Base;
//using DevExpress.XtraGrid.Views.Grid;
//using DevExpress.XtraPrinting.Native;
//using System;
//using System.Linq;
//using System.Windows.Forms;
//using XCubeCrm.Bll.Interfaces;
//using XCubeCrm.Common.Enums;
//using XCubeCrm.Model.Entities;
//using XCubeCrm.Model.Entities.Base;
//using XCubeCrm.UI.Win.Forms.FiltreForms;
//using XCubeCrm.UI.Win.Functions;
//using XCubeCrm.UI.Win.Show;
//using XCubeCrm.UI.Win.Show.Interfaces;


using XCubeCrm.Bll.Interfaces;
using XCubeCrm.Common.Enums;
using XCubeCrm.Model.Entities;
using XCubeCrm.Model.Entities.Base;
using XCubeCrm.UI.Win.Forms.FiltreForms;
using XCubeCrm.UI.Win.GenelForms;
using XCubeCrm.UI.Win.Functions;
using XCubeCrm.UI.Win.Show;
using XCubeCrm.UI.Win.Show.Interfaces;
using XCubeCrm.Bll.Interfaces;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System;
using System.Windows.Forms;
using DevExpress.Utils.Extensions;

namespace XCubeCrm.UI.Win.Forms.BaseForms
{
    public partial class BaseListForm : RibbonForm
    {
        #region Variables
        private long _filtreId;
        private bool _formSablonKayitEdilecek;
        private bool _tabloSablonKayitEdilecek;
        protected IBaseFormShow FormShow;
        protected KartTuru BaseKartTuru;
        protected bool AktifKartlariGoster = true;
        protected IBaseBll Bll;
        protected ControlNavigator Navigator;
        protected BarItem[] ShowItems;
        protected BarItem[] HideItems;
        protected internal BaseEntity SelectedEntity;
        protected internal GridView Tablo;
        protected internal bool AktifPasifButonGoster = false;
        protected internal bool RaporMu = false;
        protected internal bool MultiSelect;
        protected internal long? SeciliGelecekId;
        protected internal SelectRowFunctions RowSelect;
        protected internal string firma;
        protected internal string donem;
        protected internal string db;




        #endregion
        public BaseListForm()
        {
            InitializeComponent();

        }


        private void EventsLoad()
        {
            //Button events
            foreach (BarItem button in ribbonControl.Items)
                button.ItemClick += Button_ItemClick;

            //table events
            Tablo.DoubleClick += Tablo_DoubleClick;
            Tablo.KeyDown += Tablo_KeyDown;
            Tablo.MouseUp += Tablo_MouseUp;
            Tablo.ColumnWidthChanged += Tablo_ColumnWidthChanged;
            Tablo.ColumnPositionChanged += Tablo_ColumnPositionChanged;
            Tablo.EndSorting += Tablo_EndSorting;
            Tablo.FilterEditorCreated += Tablo_FilterEditorCreated;
            Tablo.ColumnFilterChanged += Tablo_ColumnFilterChanged;
            Tablo.CustomDrawFooterCell += Tablo_CustomDrawFooterCell;

            //form events

            Shown += BaseListForm_Shown;
            Load += BaseListForm_Load;
            FormClosing += BaseListForm_FormClosing;
            LocationChanged += BaseListForm_LocationChanged;
            SizeChanged += BaseListForm_SizeChanged;
        }


        private void SutunGizleGoster()
        {
            throw new NotImplementedException();
        }
        private void SablonKaydet()
        {
            if (_formSablonKayitEdilecek)
                Name.FormSablonKaydet(Left, Top, Width, Height, WindowState);

            if (_tabloSablonKayitEdilecek)
                Tablo.TabloSablonKaydet(IsMdiChild ? Name + " Tablosu" : Name + " TablosuMDI");
        }
        private void SablonYukle()
        {
            if (IsMdiChild)
                Tablo.TabloSablonYukle(Name + " Tablosu");
            else
            {
                Name.FormSablonYukle(this);
                Tablo.TabloSablonYukle(Name + " TablosuMDI");
            }
        }
        void ButonGizleGoster()
        {

            btnSec.Visibility = AktifPasifButonGoster ? BarItemVisibility.Never : IsMdiChild ? BarItemVisibility.Never : BarItemVisibility.Always;
            barSec.Visibility = IsMdiChild ? BarItemVisibility.Never : BarItemVisibility.Always;
            barSecAciklama.Visibility = IsMdiChild ? BarItemVisibility.Never : BarItemVisibility.Always;
            btnAktifPasifKayitlar.Visibility = AktifPasifButonGoster ? BarItemVisibility.Always : !IsMdiChild ? BarItemVisibility.Never : BarItemVisibility.Always;

            ShowItems?.ForEach(x => x.Visibility = BarItemVisibility.Always);
            /*            I
                          V
            foreach (BarItem item in ShowItems)
            {
                item.Visibility = BarItemVisibility.Always;
            }
            */
            HideItems?.ForEach(x => x.Visibility = BarItemVisibility.Never);



            if (RaporMu)
            {
                barInsert.Visibility = BarItemVisibility.Never;
                barDelete.Visibility = BarItemVisibility.Never;
                barDuzelt.Visibility = BarItemVisibility.Never;
                barKaydet.Visibility = BarItemVisibility.Never;
                barInsertAciklama.Visibility = BarItemVisibility.Never;
                barDeleteAciklama.Visibility = BarItemVisibility.Never;
                barDuzeltAciklama.Visibility = BarItemVisibility.Never;
                barKaydetAciklama.Visibility = BarItemVisibility.Never;
                btnDuzelt.Visibility = BarItemVisibility.Never;
                btnSil.Visibility = BarItemVisibility.Never;
                btnYeni.Visibility = BarItemVisibility.Never;
                btnKaydet.Visibility = BarItemVisibility.Never;
            }


        }
        protected virtual void Duzelt() { }
        private void SelectEntity()
        {
            if (MultiSelect)
            {
                //Güncellenecek
            }
            else SelectedEntity = Tablo.GetRow<BaseEntity>();

            DialogResult = DialogResult.OK;
            Close();
        }
        private void FormCaptionAyarla()
        {
            if (btnAktifPasifKayitlar == null)
            {
                Listele();
                return;
            }

            if (AktifKartlariGoster)
            {
                btnAktifPasifKayitlar.Caption = "PasifKartlar";
                Tablo.ViewCaption = Text;
            }
            else
            {
                btnAktifPasifKayitlar.Caption = "Aktif Kartlar";
                Tablo.ViewCaption = Text + " - Pasif Kartlar";
            }
            Listele();
        }
        private void IslemTuruSec()
        {
            if (!IsMdiChild)
            {
                //Güncellenecek
                SelectEntity();//mdi değilse seçim yap

            }
            //else if (IsMdiChild && RaporMu ==true)
            //{
            //    MessageBox.Show("Güncelleme açılacak");
            //}
            else 
            {
                btnDuzelt.PerformClick(); //mdi değilse içine giriş double click gibi çalış
            }
        }
        private void FiltreSec()
        {
            var entity = (Filtre)ShowListForms<FiltreListForm>.ShowDialogListForm(KartTuru.Filtre, _filtreId, BaseKartTuru, Tablo.GridControl);
            if (entity == null) return;

            _filtreId = entity.Id;
            Tablo.ActiveFilterString = entity.FiltreMetni;
        }
        protected void ShowEditFormDefault(long id)
        {
            if (id <= 0) return;
            AktifKartlariGoster = true;
            FormCaptionAyarla();
            Tablo.RowFocus("Id", id);
        }

        protected virtual void DegiskenleriDoldur()
        {

        }

        protected virtual void ShowEditForm(long id)
        {
            var result = FormShow.ShowDialogEditForm(BaseKartTuru, id);
            ShowEditFormDefault(result);
        }


        protected virtual void EntityDelete()
        {
            var entity = Tablo.GetRow<BaseEntity>();
            if (entity == null) return;
            if (!((IBaseCommonBll)Bll).Delete(entity)) return;

            Tablo.DeleteSelectedRows();
            Tablo.RowFocus(Tablo.FocusedRowHandle);
        }
        protected virtual void Listele()
        {
            //Tablo.GridControl.DataSource = ((OkulBll)Bll).List(FilterFunctions.Filter<Okul>(AktifKartlariGoster));
        }

        protected virtual void Yazdir()
        {
            TablePrintingFunctions.Yazdir(Tablo, AnaForm.SubeAdi);
        }

        protected virtual void BagliKartAc() { }
        protected internal void Yukle()
        {
            DegiskenleriDoldur();
            EventsLoad();

            Tablo.OptionsSelection.MultiSelect = MultiSelect;
            Navigator.NavigatableControl = Tablo.GridControl;

            Cursor.Current = Cursors.WaitCursor;
            Listele();
            Cursor.Current = DefaultCursor;

            //Güncellenecek
        }
        protected virtual void Button_ItemClick(object sender, ItemClickEventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;

            if (e.Item == btnDisariAktar)
            {
                var link = (BarSubItemLink)e.Item.Links[0];
                link.Focus();
                link.OpenMenu();
                link.Item.ItemLinks[0].Focus();
            }
            else if (e.Item == btnStandartExcelDosyasi)
            {
                Tablo.TabloDisariAktar(this.Text.ToString() + (Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh mm-")), DosyaTuru.ExcellStandart, e.Item.Caption, Text);

            }
            else if (e.Item == btnExcelDosyasiFormatli)
            {
                Tablo.TabloDisariAktar(this.Text.ToString() + (Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh mm-")), DosyaTuru.ExcellFormatli, e.Item.Caption, Text);
            }
            else if (e.Item == btnExcelDosyasiFormatsiz)
            {
                Tablo.TabloDisariAktar(this.Text.ToString() + (Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh mm-")), DosyaTuru.ExcellFormatsiz, e.Item.Caption);
            }
            else if (e.Item == btnWordDosyasi)
            {
                Tablo.TabloDisariAktar(this.Text.ToString() + (Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh mm-")), DosyaTuru.WordDosyasi, e.Item.Caption);
            }
            else if (e.Item == btnWordDosyasi)
            {
                Tablo.TabloDisariAktar(this.Text.ToString() + (Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh mm-")), DosyaTuru.PdfDosyasi, e.Item.Caption);
            }
            else if (e.Item == btnTxtDosyasi)
            {
                Tablo.TabloDisariAktar(this.Text.ToString() + (Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh mm-")), DosyaTuru.TextDosyasi, e.Item.Caption);
            }

            else if (e.Item == btnYeni)
            {
                //yetki kontrolü
                ShowEditForm(-1);
            }
            else if (e.Item == btnDuzelt)
            {
                

                ShowEditForm(Tablo.GetRowId());
            }
            else if (e.Item == btnSil)
            {
                //yetki kontrolü
                EntityDelete();
            }
            else if (e.Item == btnSec)
            {
                MessageBox.Show(SeciliGelecekId.ToString());
                SelectEntity();
            }
            else if (e.Item == btnYenile)
            {
                Listele();
            }
            else if (e.Item == btnFiltrele)
                FiltreSec();
            else if (e.Item == btnKolonlar)
            {
                if (Tablo.CustomizationForm == null)
                    Tablo.ShowCustomization();
                else Tablo.HideCustomization();
            }
            else if (e.Item == btnBagliKartlar)
                BagliKartAc();
            else if (e.Item == btnYazdir) Yazdir();
            else if (e.Item == btnBaskiOnizleme)
                BaskiOnizleme();
            else if (e.Item == btnTasarimDegistir)
                Duzelt();
            else if (e.Item == btnCikis) Close();
            else if (e.Item == btnAktifPasifKayitlar)
            {
                AktifKartlariGoster = !AktifKartlariGoster;
                FormCaptionAyarla();
            }

            Cursor.Current = DefaultCursor;
        }


        private void Tablo_DoubleClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            IslemTuruSec();
            Cursor.Current = DefaultCursor;
        }

        private void Tablo_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    IslemTuruSec();
                    break;

                case Keys.Escape:
                    Close();
                    break;
            }
        }

        private void Tablo_MouseUp(object sender, MouseEventArgs e)
        {
            e.SagMenuGoster(sagMenu);
        }
        // deneme




















        //deneme1






        private void Tablo_ColumnWidthChanged(object sender, ColumnEventArgs e)
        {
            _tabloSablonKayitEdilecek = true;
 
        }
        private void Tablo_ColumnPositionChanged(object sender, EventArgs e)
        {
            _tabloSablonKayitEdilecek = true;
        }
        private void Tablo_EndSorting(object sender, EventArgs e)
        {
            _tabloSablonKayitEdilecek = true;
        }
        private void Tablo_FilterEditorCreated(object sender, FilterControlEventArgs e)
        {
            e.ShowFilterEditor = false;
            ShowEditForms<FiltreEditForm>.ShowDialogEditForm(KartTuru.Filtre, _filtreId, BaseKartTuru, Tablo.GridControl);
        }
        private void Tablo_ColumnFilterChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Tablo.ActiveFilterString))
                _filtreId = 0;
        }
        private void Tablo_CustomDrawFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            if (!Tablo.OptionsView.ShowFooter) return;
            if (e.Column.Summary.Count > 0)
                e.Appearance.TextOptions.HAlignment = e.Column.ColumnEdit.Appearance.HAlignment;
        }
        private void BaseListForm_Shown(object sender, EventArgs e)
        {
            Tablo.Focus();
            ButonGizleGoster();
            // SutunGizleGoster();

            if (IsMdiChild || !SeciliGelecekId.HasValue) return; //SeciliGelecekId == null) return;
            Tablo.RowFocus("Id", SeciliGelecekId);
        }
        private void BaseListForm_Load(object sender, EventArgs e)
        {
            SablonYukle();
        }
        private void BaseListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SablonKaydet();
        }
        private void BaseListForm_LocationChanged(object sender, EventArgs e)
        {
            if (!IsMdiChild)
                _formSablonKayitEdilecek = true;
        }
        private void BaseListForm_SizeChanged(object sender, EventArgs e)
        {
            if (!IsMdiChild)
                _formSablonKayitEdilecek = true;
        }
        protected virtual void BaskiOnizleme() { }
    }
}