using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
namespace MatmaHill
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private BangChuCai bcc = new BangChuCai();

        private int[,] khoaK;

        private int[,] maTranP;
        private int[][] maTranMH;

        private int[,] maTran_ChuyenVi;
        private int[,] maTran_IK;

        private int[,] maTranC;
        private int[][] maTranGM;

        private string duLieu_Da_MH = "";
        private string dulieu_Da_GM = "";
        private int capMaTran = 0;
        private int doDaiChuoi = 0;

        private int det_K = 0;
        private int I_det_K = 0;
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtL.Clear();
            rtbCyphertext.Clear();
            rtbKhoaIK.Clear();
            rtbKhoaK.Clear();
            rtbPlantext.Clear();
            khoaK = null;

            maTranP = null;
            maTranMH = null;

            maTran_ChuyenVi = null;
            maTran_IK = null;

            maTranC = null;
            maTranGM = null;

            duLieu_Da_MH = "";
            dulieu_Da_GM = "";
            capMaTran = 0;
            doDaiChuoi = 0;

            det_K = 0;
            I_det_K = 0;
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnSinhK_Click(object sender, EventArgs e)
        {
            rtbKhoaK.Text = "";
            
            if(string.IsNullOrEmpty(txtL.Text))
            {
                MessageBox.Show("Bạn chưa nhập độ dài khoá!");
            }
            else
            {
                try
                {
                    capMaTran = int.Parse(txtL.Text);
                    khoaK = new int[capMaTran, capMaTran];
                    Random rd = new Random();
                    for (int i = 0; i < capMaTran; i++)
                    {
                        for (int j = 0; j < capMaTran; j++)
                        {
                            khoaK[i, j] = rd.Next(0, bcc.bangASCII.Length - 1);
                        }

                    }
                    rtbKhoaK.Text = show_MaTran(2, "Ma tran K");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi : " + ex.Message);
                }
            }    
        }

        private void btnSinhIK_Click(object sender, EventArgs e)
        {
            if (khoaK == null)
            {
                MessageBox.Show("Hãy sinh khóa K hoặc mở 1 file dữ liệu", "Lỗi");
            }
            else
            {
                Tim_Ma_Tran_Chuyen_Vi();
                Tim_Ma_Tran_Nghich_Dao_IK();
                rtbKhoaIK.Text += "\r\n";
                rtbKhoaIK.Text += show_MaTran(4, "Ma tran I_K");
            }
        }
        private Boolean checkChuoi(string inputS)
        {
            foreach(char character in inputS)
            {
                string strCharacter = character.ToString();
                if(!bcc.bangASCII.Contains(strCharacter))
                {
                    return false;
                }    
            }
            return true;
        }
        private Boolean xu_Ly_Du_Lieu_Ma_Hoa(string inputS)
        {
            if(checkChuoi(inputS))
            {
                string[] tmp = null;
                int k = 0;
                if (inputS.Contains(","))
                {
                    tmp = inputS.Split(',');
                    doDaiChuoi = tmp.Length;
                }
                else
                {
                    doDaiChuoi = inputS.Length;
                }

                if (doDaiChuoi > 0)
                {
                    if (doDaiChuoi % capMaTran != 0)
                    {
                        int soLuongThem = capMaTran - (doDaiChuoi % capMaTran);
                        inputS += new string('X', soLuongThem);
                        doDaiChuoi = inputS.Length;
                    }
                    maTranP = new int[doDaiChuoi / capMaTran, capMaTran];
                    if (inputS.Contains(","))
                    {
                        for (int i = 0; i < capMaTran; i++)
                        {
                            for (int j = 0; j < capMaTran; j++)
                            {
                                maTranP[i, j] = int.Parse(tmp[k]);
                                k++;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < doDaiChuoi; i++)
                        {
                            char c = inputS[i].ToString()[0];
                            int viTriC = Array.IndexOf(bcc.bangASCII, c.ToString());

                            int row = i / capMaTran;
                            int col = i % capMaTran;

                            maTranP[row, col] = viTriC;

                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }    
            else
            {
                return false;
            }    
        }
        private Boolean maHoaHill()
        {
            if (khoaK != null && maTranP != null)
            {
                maTranMH = new int[doDaiChuoi / capMaTran][];
                for (int i = 0; i < doDaiChuoi / capMaTran; i++)
                {
                    int[] vector = new int[capMaTran];
                    int[] tmp = new int[capMaTran];

                    for (int j = 0; j < capMaTran; j++)
                    {
                        vector[j] = maTranP[i, j];
                    }

                    for (int k = 0; k < capMaTran; k++)
                    {
                        tmp[k] = 0;
                        for (int z = 0; z < capMaTran; z++)
                        {
                            tmp[k] += vector[z] * khoaK[k, z];
                        }
                    }

                    maTranMH[i] = tmp.ToArray<int>();
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        private void showMaHoa()
        {
            rtbCyphertext.Clear();
            for (int l = 0; l < doDaiChuoi / capMaTran; l++)
            {
                for (int m = 0; m < capMaTran; m++)
                {
                    duLieu_Da_MH += bcc.bangASCII[maTranMH[l][m] % 97];
                }
            }
            rtbCyphertext.Text = duLieu_Da_MH;
        }

        private string show_MaTran(int soThuTu, string tenMaTran)
        {
            string chuoi_MaTran = tenMaTran + "\r\n";
            if (soThuTu == 1)
            {
                for (int i = 0; i < doDaiChuoi / capMaTran; i++)
                {
                    for (int j = 0; j < capMaTran; j++)
                    {
                        chuoi_MaTran += maTranP[i, j] + "\t";
                    }
                    chuoi_MaTran += "\r\n";
                }
            }
            else if (soThuTu == 2)
            {
                for (int i = 0; i < capMaTran; i++)
                {
                    for (int j = 0; j < capMaTran; j++)
                    {
                        chuoi_MaTran += khoaK[i, j].ToString() + "\t";
                    }
                    chuoi_MaTran += "\r\n";
                }
            }
            else if (soThuTu == 3)
            {
                for (int z1 = 0; z1 < capMaTran; z1++)
                {
                    for (int z2 = 0; z2 < capMaTran; z2++)
                    {
                        chuoi_MaTran += maTran_ChuyenVi[z1, z2] + "\t";
                    }
                    chuoi_MaTran += "\r\n";
                }
            }
            else if (soThuTu == 4)
            {
                for (int i = 0; i < capMaTran; i++)
                {
                    for (int j = 0; j < capMaTran; j++)
                    {
                        chuoi_MaTran += maTran_IK[i, j] + "\t";
                    }
                    chuoi_MaTran += "\r\n";
                }
            }
            if (soThuTu == 5)
            {
                for (int i = 0; i < doDaiChuoi / capMaTran; i++)
                {
                    for (int j = 0; j < capMaTran; j++)
                    {
                        chuoi_MaTran += maTranC[i, j] + "\t";
                    }
                    chuoi_MaTran += "\r\n";
                }
            }
            return chuoi_MaTran;
        }

        private int[,] tim_MT_Con(int[,] mat, int p, int q)
        {
            int n = mat.GetLength(0);

            int[,] temp = new int[n - 1, n - 1];
            int i = 0, j = 0;

            for (int row = 0; row < n; row++)
            {
                for (int col = 0; col < n; col++)
                {
                    if ((row != p) && (col != q))
                    {
    
                        temp[i, j++] = mat[row, col];

                        if (j == n - 1)
                        {
                            j = 0;
                            i++;
                        }
                    }
                }
            }
            return temp;
        }
        private int Tinh_Dinh_Thuc(int[,] mat)
        {

            int n = mat.GetLength(0);
            if (n == 2)
            {
                return (mat[0, 0] * mat[1, 1]) - (mat[0, 1] * mat[1, 0]);
            }

            int det = 0;

            for (int col = 0; col < n; col++)
            {
                int sign = (col % 2 == 0) ? 1 : -1;
                int[,] submatrix = tim_MT_Con(mat, 0, col);
                int submatrixDet = Tinh_Dinh_Thuc(submatrix);

                det += sign * mat[0, col] * submatrixDet;

            }
            det = det % 97;
            if (det < 0) det = det + bcc.bangASCII.Length;
            return det;
        }
        private int Tinh_Nghich_Dao_Dinh_Thuc(int do_Dai_Bang_Chu_Cai, int dinh_Thuc)
        {
            int a1 = 1, a2 = 0, a3 = do_Dai_Bang_Chu_Cai;
            int b1 = 0, b2 = 1, b3 = dinh_Thuc;
            while (b3 != 0 && b3 != 1)
            {
                int q = a3 / b3;
                int t1 = a1 - q * b1;
                int t2 = a2 - q * b2;
                int t3 = a3 - q * b3;
                a1 = b1; a2 = b2; a3 = b3;
                b1 = t1; b2 = t2; b3 = t3;
                if (b3 == 1)
                {
                    break;
                }
            }
            if (b3 == 1)
            {
                if (b2 < 0)
                {
                    b2 += do_Dai_Bang_Chu_Cai;
                    return b2;
                }
                else
                {
                    return b2;
                }
            }
            return 0;
        }
        private void Tim_Ma_Tran_Chuyen_Vi()
        {
            maTran_ChuyenVi = new int[capMaTran, capMaTran];
            if (capMaTran == 2)
            {
                maTran_ChuyenVi[0, 0] = khoaK[1, 1];
                maTran_ChuyenVi[0, 1] = (-khoaK[0, 1]);
                maTran_ChuyenVi[1, 0] = (-khoaK[1, 0]);
                maTran_ChuyenVi[1, 1] = khoaK[0, 0];
            }
            else
            {
                for (int i = 0; i < capMaTran; i++)
                {
                    for (int j = 0; j < capMaTran; j++)
                    {
                        int[,] mtTMP = new int[capMaTran - 1, capMaTran - 1];
                        mtTMP = tim_MT_Con(khoaK, i, j);
                        maTran_ChuyenVi[i, j] = (int)Math.Pow(-1, (i + j)) * Tinh_Dinh_Thuc(mtTMP);
                    }
                }
            }

            rtbKhoaIK.Text += show_MaTran(3, "Ma tran chuyen vi");
        }
        private void Tim_Ma_Tran_Nghich_Dao_IK()
        {
            maTran_IK = new int[capMaTran, capMaTran];
            det_K = Tinh_Dinh_Thuc(khoaK);
            I_det_K = Tinh_Nghich_Dao_Dinh_Thuc(bcc.bangASCII.Length, det_K);
            if (capMaTran != 2)
            {
                rtbKhoaIK.Text += "Det_K: " + det_K + "\r\n" + "I_DET_K: " + I_det_K + "\r\n";
                for (int i = 0; i < capMaTran; i++)
                {
                    for (int j = 0; j < capMaTran; j++)
                    {
                        int tmpTich = maTran_ChuyenVi[i, j] * I_det_K;
                        maTran_IK[j, i] = (tmpTich % bcc.bangASCII.Length);
                        if (maTran_IK[j, i] < 0)
                        {
                            maTran_IK[j, i] = maTran_IK[j, i] + bcc.bangASCII.Length;
                        }
                    }
                }
            }
            else
            {
                maTran_IK[0, 0] = ((maTran_ChuyenVi[0, 0]) * I_det_K) % bcc.bangASCII.Length;
                maTran_IK[0, 1] = ((maTran_ChuyenVi[0, 1] + bcc.bangASCII.Length) * I_det_K) % bcc.bangASCII.Length;
                maTran_IK[1, 0] = ((maTran_ChuyenVi[1, 0] + bcc.bangASCII.Length) * I_det_K) % bcc.bangASCII.Length;
                maTran_IK[1, 1] = ((maTran_ChuyenVi[1, 1]) * I_det_K) % bcc.bangASCII.Length;
            }
        }
        private Boolean xu_Ly_Du_Lieu_Giai_Ma(string inputS)
        {
            doDaiChuoi = inputS.Length;
            if (doDaiChuoi == 0)
            {
                rtbKhoaK.Clear();
                MessageBox.Show("Dữ liệu giải mã không hợp lệ", "Lỗi");
            }
            else
            {
                if ((doDaiChuoi % capMaTran) != 0)
                {
                    MessageBox.Show("Độ dài chuỗi không phù hợp tạo ma trận [" + doDaiChuoi / capMaTran + "," + capMaTran + "]");
                }
                else
                {
                    maTranC = new int[doDaiChuoi / capMaTran, capMaTran];
                    for (int i = 0; i < doDaiChuoi; i++)
                    {
                        char c = inputS[i].ToString()[0];
                        int viTriC = Array.IndexOf(bcc.bangASCII, c.ToString());

                        int row = i / capMaTran;
                        int col = i % capMaTran;

                        maTranC[row, col] = viTriC;
                    }
                    return true;
                }

            }
            return false;
        }
        private Boolean giaiMaHill()
        {
            if (maTran_IK != null && maTranC != null)
            {
                maTranGM = new int[doDaiChuoi / capMaTran][];
                for (int i = 0; i < doDaiChuoi / capMaTran; i++)
                {
                    int[] vector = new int[capMaTran];
                    int[] tmp = new int[capMaTran];

                    for (int j = 0; j < capMaTran; j++)
                    {
                        vector[j] = maTranC[i, j];
                    }

                    for (int k = 0; k < capMaTran; k++)
                    {
                        tmp[k] = 0;
                        for (int z = 0; z < capMaTran; z++)
                        {
                            tmp[k] += vector[z] * maTran_IK[k, z];
                        }
                    }

                    maTranGM[i] = tmp.ToArray<int>();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void showGiaiMa()
        {
            rtbPlantext.Clear();
            for (int l = 0; l < doDaiChuoi / capMaTran; l++)
            {
                for (int m = 0; m < capMaTran; m++)
                {
                    dulieu_Da_GM += bcc.bangASCII[maTranGM[l][m] % 97];

                }
            }
            rtbPlantext.Text = dulieu_Da_GM;
        }
        private void btnOpenfileP_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Chọn tệp dữ liệu đầu vào";
            openFileDialog.Filter = "Tất cả các tệp (*.*)|*.*|Tệp văn bản (*.txt)|*.txt|Tệp mã hóa Hill (*.hill)|*.hill";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                string fileContent = File.ReadAllText(filePath);
                rtbPlantext.Text = fileContent;
            }
        }

        private void btnOpenfileC_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Chọn tệp dữ liệu đầu vào";
            openFileDialog.Filter = "Tất cả các tệp (*.*)|*.*|Tệp văn bản (*.txt)|*.txt|Tệp mã hóa Hill (*.hill)|*.hill";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                string fileContent = File.ReadAllText(filePath);
                rtbCyphertext.Text = fileContent;
            }
        }
        private void btnMahoa_Click(object sender, EventArgs e)
        {
            rtbCyphertext.Clear();
            rtbKhoaIK.Clear();
            string chuoiInput = rtbPlantext.Text;

            if (xu_Ly_Du_Lieu_Ma_Hoa(chuoiInput))
            {
                if (maHoaHill())
                {
                    MessageBox.Show("Mã hóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rtbPlantext.Text += show_MaTran(1, "");
                    showMaHoa();
                }
                else
                {
                    MessageBox.Show("Mã hóa không thành công, hãy kiểm tra KHÓA và DỮ LIỆU", "Lỗi");
                }
            }
            else
            {
                rtbPlantext.Clear();
                MessageBox.Show("Xử lý dữ liệu mã hóa không thành công", "Lỗi");
            }
            //Console.WriteLine(chuoiMaHoa);
        }

        private void btnGiaima_Click(object sender, EventArgs e)
        {
            rtbPlantext.Clear();
            string chuoiMaHoa = rtbCyphertext.Text;

            if (xu_Ly_Du_Lieu_Giai_Ma(chuoiMaHoa))
            {
                if (giaiMaHill())
                {
                    MessageBox.Show("Giải mã thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rtbCyphertext.Text += show_MaTran(5, "");
                    showGiaiMa();
                }
                else
                {
                    MessageBox.Show("Giải mã thất bại, Kiểm tra lại quá trình sinh IK và DỮ LIỆU", "Lỗi");
                }
            }
            else
            {
                rtbCyphertext.Clear();
                MessageBox.Show("Xử lý dữ liệu giải mã không thành công", "Lỗi");
            }
            
            Console.WriteLine(chuoiMaHoa);
        }
    }
}
