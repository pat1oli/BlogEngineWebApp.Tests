using BlogEngineWebApp.Dto;
using BlogEngineWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogEngineWebApp.Tests.Helper

{
    public static class  Utility
    {
        public static Post CreateCustomPost()
        {
            return new Post { PostId = 1, Content = "content", Title = "title", PublicationDate = DateTime.Today };
        }

        public static PostDto CreateCustomPostDto()
        {
            return new PostDto { PostId = 1, Content = "content", Title = "title" };
        }
    }
}
