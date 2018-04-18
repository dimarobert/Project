using AutoMapper;
using Project.Account.Services;
using Project.UserProfileDomain.Repositories;
using Project.ViewModels.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Project.Controllers {

    [RoutePrefix("api")]
    public class JsonApiController : ApiController {

        //IUserService userService;
        //IInterestRepository interestRepository;

        //public JsonApiController(IUserService userService, IInterestRepository interestRepository) {
        //    this.userService = userService;
        //    this.interestRepository = interestRepository ?? throw new ArgumentNullException(nameof(interestRepository));
        //}

    }
}
