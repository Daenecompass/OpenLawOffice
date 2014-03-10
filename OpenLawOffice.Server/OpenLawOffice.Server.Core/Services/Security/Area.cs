﻿using System;
using System.Collections.Generic;
using System.Data;
using ServiceStack.OrmLite;

namespace OpenLawOffice.Server.Core.Services.Security
{
    public class Area
        : ResourceBase<Common.Models.Security.Area, DBOs.Security.Area,
            Rest.Requests.Security.Area, Common.Rest.Responses.Security.Area>
    {
        public override List<DBOs.Security.Area> GetList(Rest.Requests.Security.Area request, IDbConnection db)
        {
            string filterClause = "";
            int parentid = 0;

            if (!string.IsNullOrEmpty(request.Name))
                filterClause += " LOWER(\"name\") like '%' || LOWER(@Name) || '%' AND";

            if (!request.ShowAll.HasValue || !request.ShowAll.Value)
            { 
                // honor parent
                if (request.ParentId.HasValue && request.ParentId.Value > 0)
                {
                    filterClause += " \"parent_id\"=@ParentId AND";
                    parentid = request.ParentId.Value;
                }
                else
                    filterClause += " \"parent_id\" is null AND";
            }

            filterClause += " \"utc_disabled\" is null";

            return db.SqlList<DBOs.Security.Area>("SELECT * FROM \"area\" WHERE" + filterClause,
                new { Name = request.Name, ParentId = parentid });
        }
    }
}
