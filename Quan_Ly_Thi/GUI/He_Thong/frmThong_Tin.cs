﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Quan_Ly_Thi.BUS;
using Quan_Ly_Thi.DTO;
using Quan_Ly_Thi.DAO;
using Quan_Ly_Thi.Validator;

namespace Quan_Ly_Thi.GUI.He_Thong
{
    public partial class frmThong_Tin : Form
    {
        string loi = "";

        public frmThong_Tin()
        {
            InitializeComponent();

        }
        RegexValidator UserAccount = new RegexValidator();
        RegexValidator UserCMND_TCC = new RegexValidator();
        private void frmThong_Tin_Load(object sender, EventArgs e)
        {
            //Quyền
            using (var QLTTN = new QLTTNDataContext())
            {
                foreach (var item in QLTTN.PHANQUYENs)
                {
                    cbb_Quyen.Items.Add(item.TenPhanQuyen);
                }
            }
            cbb_Quyen.SelectedIndex = 0;


            UserAccount = new RegexValidator();
            UserAccount.ControlToValidate = txtSudent_code;
            UserAccount.ErrorMessage = "Account Name Incorrect, Correct Form: TK000001";
            UserAccount.Pattern = @"^[A-X][A-Z0-9]{0,9}$";

            UserCMND_TCC = new RegexValidator();
            UserCMND_TCC.ControlToValidate = txtCMND;
            UserCMND_TCC.ErrorMessage = "CMND Incorrect, Correct Form: 123456789";
            UserCMND_TCC.Pattern = @"^[0-9]{10,15}$";

            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            loi = UserAccount.ErrorMessage;
            loi = UserCMND_TCC.ErrorMessage;
            if (loi == "")
            {
                NGUOIDUNG nguoi_dung = new NGUOIDUNG();
                nguoi_dung.TaiKhoan = txtSudent_code.Text;
                nguoi_dung.MatKhau = txtPassword.Text;
                nguoi_dung.HoTen = txtStudent_Name.Text;
                nguoi_dung.CMND_TCC = txtCMND.Text;
                nguoi_dung.NgaySinh = dpStudent_birth_date.Value;


                if (cbb_Quyen.Text == "HỌC SINH")
                {

                    if (txtStudent_class.Text != "")
                    {
                        nguoi_dung.MaLop = BUS_Tai_Khoan.ID_Lop(txtStudent_class.Text);
                    }
                }
                else if (cbb_Quyen.Text == "GIÁO VIÊN")
                {

                    if (txtStudent_class.Text != "")
                    {
                        nguoi_dung.MaKhoi = BUS_Tai_Khoan.ID_Khoi(txtStudent_class.Text);

                    }
                }


                nguoi_dung.MaPhanQuyen = BUS_Tai_Khoan.ID_Quyen(cbb_Quyen.Text);
                BUS_Tai_Khoan.Them_nguoi_dung(nguoi_dung);

                txtCMND.Clear();
                txtPassword.Clear();
                txtStudent_class.Clear();
                txtStudent_Name.Clear();
                txtSudent_code.Clear();
            }
            else
            {
                MessageBox.Show("Chưa Hợp Lệ", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }



        }

        private void cbb_Quyen_SelectedValueChanged(object sender, EventArgs e)
        {
            UserAccount = new RegexValidator();
            UserAccount.ControlToValidate = txtSudent_code;
            UserAccount.ErrorMessage = "Account Name Incorrect, Correct Form: TK000001";
            UserAccount.Pattern = @"^[A-X][A-Z0-9]{0,9}$";

            UserCMND_TCC = new RegexValidator();
            UserCMND_TCC.ControlToValidate = txtCMND;
            UserCMND_TCC.ErrorMessage = "CMND Incorrect, Correct Form: 123456789";
            UserCMND_TCC.Pattern = @"^[0-9]{10,15}$";

            string text = "";
            txtStudent_class.Visible = true;
            txtStudent_class.AutoCompleteCustomSource.Clear();


            if (cbb_Quyen.Text == "HỌC SINH")
            {
                text = "Lớp:";
                //Lớp
                using (var QLTTN = new QLTTNDataContext())
                {
                    foreach (var item in QLTTN.LOPHOCs)
                    {
                        txtStudent_class.AutoCompleteCustomSource.Add(item.TenLop);
                    }
                }
            }
            else if(cbb_Quyen.Text == "ADMIN")
            {
                txtStudent_class.Visible = false;
                text = "";
            }
            else if (cbb_Quyen.Text =="GIÁO VIÊN")
            {
                text = "Khối:";
                //Khối
                using (var QLTTN = new QLTTNDataContext())
                {
                    foreach (var item in QLTTN.KHOIs)
                    {
                        txtStudent_class.AutoCompleteCustomSource.Add(item.TenKhoi);
                    }
                }
            }
            label5.Text = text;
        }

    }
}
