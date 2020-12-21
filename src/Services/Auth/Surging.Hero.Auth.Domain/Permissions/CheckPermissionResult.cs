using Surging.Hero.Auth.Domain.Shared;

namespace Surging.Hero.Auth.Domain.Permissions
{
    public class CheckPermissionResult
    {

        public CheckPermissionResult(DataPermissionType dataPermissionType)
        {
            DataPermissionType = dataPermissionType;
        }
        

        public DataPermissionType DataPermissionType { get; }

        private long[] _dataPermissionOrgIds;
        public long[] DataPermissionOrgIds
        {
            get
            {
                if (DataPermissionType == DataPermissionType.AllOrg)
                {
                    return null;
                }

                return _dataPermissionOrgIds;
            }
            set
            {
                _dataPermissionOrgIds = value;
            }
        }
    }
}