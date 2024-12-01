using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SimpleBankGUI
{
    public partial class BankForm : Form
    {
        public class Bank
        {
            public string accountId;
            public string accountNumber;
            public string accountName;
            public double balance;
            public string accountType;
            public static List<Bank> accounts = new List<Bank>();

            public void CreateAccount(string accountType, string accountName, double initialBalance, string accountNumber)
            {
                this.accountType = accountType;
                this.accountName = accountName;
                this.balance = initialBalance;
                this.accountNumber = accountNumber;
                this.accountId = Guid.NewGuid().ToString();
                accounts.Add(this);
            }

            public static Bank GetAccountByNumber(string accountNumber)
            {
                return accounts.Find(a => a.accountNumber == accountNumber);
            }
        }

        public BankForm()
        {
            InitializeComponent();
        }

        private void BankForm_Load(object sender, EventArgs e)
        {
            accountTypeComboBox.Items.Add("Debit");
            accountTypeComboBox.Items.Add("Credit");
            accountTypeComboBox.Items.Add("Savings");
            accountTypeComboBox.SelectedIndex = 0;
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            string accountType = accountTypeComboBox.SelectedItem.ToString();
            string accountName = txtAccountName.Text;
            string accountNumber = txtAccountNumber.Text;
            double initialBalance;

            if (string.IsNullOrWhiteSpace(accountName) || string.IsNullOrWhiteSpace(accountNumber) || !double.TryParse(txtInitialBalance.Text, out initialBalance) || initialBalance < 0)
            {
                MessageBox.Show("Invalid inputs. Please check your fields.");
                return;
            }

            Bank newAccount = new Bank();
            newAccount.CreateAccount(accountType, accountName, initialBalance, accountNumber);

            MessageBox.Show("Account Created Successfully!");
        }

        private void btnShowAccountInfo_Click(object sender, EventArgs e)
        {
            string accountNumber = txtSearchAccountNumber.Text;

            Bank account = Bank.GetAccountByNumber(accountNumber);
            if (account != null)
            {
                lblAccountInfo.Text = "Account ID: " + account.accountId + "\n" +
                                      "Account Number: " + account.accountNumber + "\n" +
                                      "Account Name: " + account.accountName + "\n" +
                                      "Account Type: " + account.accountType + "\n" +
                                      "Balance: " + account.balance;
            }
            else
            {
                MessageBox.Show("Account not found.");
            }
        }

        private void btnDeposit_Click(object sender, EventArgs e)
        {
            string accountNumber = txtDepositAccountNumber.Text;
            Bank account = Bank.GetAccountByNumber(accountNumber);

            if (account != null)
            {
                double depositAmount;
                if (!double.TryParse(txtDepositAmount.Text, out depositAmount) || depositAmount <= 0)
                {
                    MessageBox.Show("Invalid deposit amount.");
                    return;
                }
                account.balance += depositAmount;
                MessageBox.Show("Deposit successful! New Balance: " + account.balance);
            }
            else
            {
                MessageBox.Show("Account not found.");
            }
        }

        private void btnWithdraw_Click(object sender, EventArgs e)
        {
            string accountNumber = txtWithdrawAccountNumber.Text;
            Bank account = Bank.GetAccountByNumber(accountNumber);

            if (account != null)
            {
                double withdrawAmount;
                if (!double.TryParse(txtWithdrawAmount.Text, out withdrawAmount) || withdrawAmount <= 0)
                {
                    MessageBox.Show("Invalid withdrawal amount.");
                    return;
                }

                if (withdrawAmount > account.balance)
                {
                    MessageBox.Show("Insufficient balance.");
                }
                else
                {
                    account.balance -= withdrawAmount;
                    MessageBox.Show("Withdrawal successful! New Balance: " + account.balance);
                }
            }
            else
            {
                MessageBox.Show("Account not found.");
            }
        }
    }
}
