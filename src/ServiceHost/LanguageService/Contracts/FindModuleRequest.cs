﻿//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using System.Collections.Generic;
using Microsoft.SqlTools.ServiceLayer.ServiceHost.Protocol.Contracts;

namespace Microsoft.SqlTools.ServiceLayer.LanguageService.Contracts
{
    public class FindModuleRequest
    {
        public static readonly
            RequestType<List<PSModuleMessage>, object> Type =
            RequestType<List<PSModuleMessage>, object>.Create("SqlTools/findModule");
    }


    public class PSModuleMessage
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
