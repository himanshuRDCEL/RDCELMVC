using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraspCorn.Common.Enums;
using GraspCorn.Common.Helper;

using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Helper;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.UniversalPricemasterDetails;

namespace RDCEL.DocUpload.BAL.CouponManager
{
   public class CouponManager
    {
        CouponRepository _couponRepository;
        CouponMasterRepository _couponMasterRepository;
    }
}
