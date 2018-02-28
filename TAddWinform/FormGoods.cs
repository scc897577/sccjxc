﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace TAddWinform {

    /// <summary>
    /// 所有的窗体继承自FormMdiBase
    /// </summary>
    public partial class FormGoods : FormMdiBase {
        public FormGoods() {
            InitializeComponent();
        }

        /// <summary>
        /// 表单加载的时候查询所有的商品列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormGoods_Load(object sender, EventArgs e) {
            LoadAllGoods();
            //GridView为不可编辑状态
            gridView1.OptionsBehavior.Editable = false;
        }
        /// <summary>
        /// 查询所有的商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadAllGoods();
        }

        private void LoadAllGoods()
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append("select g.*,f.GoodsFromName as fname,c.GoodsCategoryName as cname from " +
                      Program.DataBaseName + "..MD_Goods as g" +
                      " inner join " + Program.DataBaseName + "..MD_GoodsFrom as f on g.GoodsFromID=f.Id" +
                      " inner join " + Program.DataBaseName + "..MD_GoodsCategory as c on g.GoodsCategoryId=c.id" +
                      " where g.Actived=1");
            DataTable table = DbHelperSQL.Query(sb.ToString()).Tables[0];
            List<Goods> goodses = new List<Goods>();
            foreach (DataRow row in table.Rows) {
                goodses.Add(new Goods() {
                    Id = Convert.ToInt32(row["Id"]),
                    GoodsCode = row["GoodsCode"].ToString(),
                    GoodsName = row["GoodsName"].ToString(),
                    GoodsFromTxt = row["fname"].ToString(),
                    GoodsCategoryTxt = row["cname"].ToString()
                });
            }
            gridControl1.DataSource = goodses;
        }

        /// <summary>
        /// 添加商品的产地和种类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            FormAddFromAndCategory form = new FormAddFromAndCategory();
            form.StartPosition = FormStartPosition.CenterParent;
            form.Show();
        }
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            FormGoodsWhere formGoodsWhere = new FormGoodsWhere();
            formGoodsWhere.SelectGoodsesEvent+=formGoodsWhere_SelectGoodsesEvent;
            formGoodsWhere.Show();
        }
        /// <summary>
        /// 根据条件查询商品信息列表
        /// </summary>
        /// <param name="goods"></param>
        public void formGoodsWhere_SelectGoodsesEvent(Goods goods)
        {
            string sql = "select a.*,b.GoodsFromName as gf,c.GoodsCategoryName as gc from "
                          +Program.DataBaseName +"..MD_Goods as a" +
                         " inner join " + Program.DataBaseName + "..MD_GoodsFrom as b on a.GoodsFromId=b.Id" +
                         " inner join " + Program.DataBaseName + "..MD_GoodsCategory as c on a.GoodsCategoryId=c.Id"+
                         " where a.Actived=1";
            if (!string.IsNullOrEmpty(goods.GoodsCode))
            {
                sql += " and a.GoodsCode=" + "'" + goods.GoodsCode + "'";
            }
            if (!string.IsNullOrEmpty(goods.GoodsName))
            {
                sql += " and a.GoodsName like " + "'%" + goods.GoodsName + "%'";
            }
            if (goods.GoodsFromId>0)
            {
                sql += " and a.GoodsFromId=" + goods.GoodsFromId;
            }

            if (goods.GoodCategoryId>0)
            {
                sql += " and a.GoodsCategoryId=" + goods.GoodCategoryId;
            }

            DataTable table = DbHelperSQL.Query(sql).Tables[0];
            List<Goods> list = new List<Goods>();
            if (table.Rows.Count>0)
            {
                foreach (DataRow row in table.Rows) {
                    list.Add(new Goods() {
                        Id = Convert.ToInt32(row["Id"]),
                        GoodsCode = row["GoodsCode"].ToString(),
                        GoodsName = row["GoodsName"].ToString(),
                        GoodsCategoryTxt = row["gc"].ToString(),
                        GoodsFromTxt = row["gf"].ToString()
                    });
                }
            }

            gridControl1.DataSource = list;
        }

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            FormAddGoods formAddGoods = new FormAddGoods();
            formAddGoods.SelectAllGoodsesEvent+=formAddGoods_SelectAllGoodsesEvent;
            formAddGoods.Show();
        }


        public void formAddGoods_SelectAllGoodsesEvent()
        {
            LoadAllGoods();
        }
    }
}