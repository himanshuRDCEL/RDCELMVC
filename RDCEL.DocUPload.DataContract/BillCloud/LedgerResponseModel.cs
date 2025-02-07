using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.BillCloud
{
    //public class LedgerResponseModel
    //{
    //    public LedgerResponseData data { get; set; }
    //}

    //public class Account
    //{
    //    public int account_type_id { get; set; }
    //    public AccountType account_type { get; set; }
    //    public Entity entity { get; set; }
    //}

    //public class AccountType
    //{
    //    public string type_name { get; set; }
    //}

    //public class LedgerResponseData
    //{
    //    public List<Ledger> ledger { get; set; }
    //}

    //public class Entity
    //{
    //    public string entity_name { get; set; }
    //    public string entity_ref_id { get; set; }
    //    public Role role { get; set; }
    //}

    //public class Ledger
    //{
    //    public int id { get; set; }
    //    public object created_at { get; set; }
    //    public string ledger_date { get; set; }
    //    public int account_id { get; set; }
    //    public int opening_balance { get; set; }
    //    public int debit_amount { get; set; }
    //    public int credit_amount { get; set; }
    //    public int closing_balance { get; set; }
    //    public string narration { get; set; }
    //    public Voucher voucher { get; set; }
    //    public Account account { get; set; }
    //}

    //public class Role
    //{
    //    public string role_name { get; set; }
    //}
    //public class Voucher
    //{
    //    public string voucher_id { get; set; }
    //}



    ///////Updtaed Response For Ledger
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Account
    {
        public int account_type_id { get; set; }
        public AccountType account_type { get; set; }
        public Member member { get; set; }
    }

    public class AccountType
    {
        public string type_name { get; set; }
    }

    public class Ledger
    {
        public int id { get; set; }
        public long created_at { get; set; }
        public string ledger_date { get; set; }
        public int voucher_id { get; set; }
        public int account_id { get; set; }
        public int opening_balance { get; set; }
        public int debit_amount { get; set; }
        public int credit_amount { get; set; }
        public int closing_balance { get; set; }
        public string narration { get; set; }
        public object settlement_batch_entry_id { get; set; }
        public object settlement_no { get; set; }
        public Account account { get; set; }
        public Voucher voucher { get; set; }
    }

    public class Member
    {
        public string role_ref_id { get; set; }
        public string membership { get; set; }
        public Role role { get; set; }
    }

    public class Role
    {
        public string role_name { get; set; }
    }

    public class LedgerResponseModel
    {
        public List<Ledger> data { get; set; }
    }

    public class Voucher
    {
        public string voucher_id { get; set; }
    }


}
