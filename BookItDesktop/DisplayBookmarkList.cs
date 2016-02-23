using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookItDesktop
{
    class DisplayBookmarkList: BookIT
    {
        List<ModelBookmarTag> mbt;
     
        int recieveId;
        public DisplayBookmarkList()
        {
            mbt = new List<ModelBookmarTag>();
        }

        public DisplayBookmarkList(int id)
        {
            recieveId = id;
        }
        public DisplayBookmarkList(ModelBookmarTag mbl)
        {
            SaveEdit(mbl);
        }

        public List<ModelBookmarTag> getBookmarks()
        {
            Entities ent = new Entities();
            List<Bookmarks> a1 = (from a in ent.Bookmarks where a.UserId == anu.Id select a).ToList();
            foreach (Bookmarks a in a1)
            {
                ModelBookmarTag dbt = new ModelBookmarTag();
                dbt.bookmarkedPage = a;
                dbt.tagList = new List<Tags>();
                List<Tags> tem1 = (from aa in ent.BookmarkTags where aa.Bookmarks.BookmarkID == a.BookmarkID select aa.Tags).ToList();
                for (int i = 0; i < tem1.Count; i++)
                {
                    dbt.tagList.Add(tem1[i]);
                }
                mbt.Add(dbt);

            }
            return mbt;
        }

        public ModelBookmarTag getSingleBookmark()
        {
            Entities ent = new Entities();
            ModelBookmarTag mbt1 = new ModelBookmarTag();
            mbt1.tagList = new List<Tags>();
            mbt1.bookmarkedPage = (from aa in ent.Bookmarks where recieveId == aa.BookmarkID select aa).Single();
            mbt1.tagList = (from aa in ent.BookmarkTags where aa.Bookmark_BookmarkID == mbt1.bookmarkedPage.BookmarkID select aa.Tags).ToList();
            return mbt1;
        }

        public void SaveEdit(ModelBookmarTag bkt)
        {
            Entities dbContext = new Entities();
            var test = dbContext.Entry(bkt.bookmarkedPage);
            dbContext.Bookmarks.Attach(bkt.bookmarkedPage);
            test.Property(i => i.Url).IsModified = true;
            for (int i = 0; i < bkt.tagList.Count; i++)
            {
                dbContext.Tags.Attach(bkt.tagList[i]);
                var test1 = dbContext.Entry(bkt.tagList[i]);
                test1.Property(j => j.TagName).IsModified = true;
            }
            dbContext.SaveChanges();

        }

        public void SaveBookmark(string url, string tags,string userID)
        {
            string[] tag = tags.Split(',');
            Bookmarks book1 = new Bookmarks();
            book1.UserId = userID;
            book1.Url = url;
            
            Entities ent = new Entities();
            foreach (string ss in tag)
            {
                Tags tag1 = new Tags();
                tag1.TagName = ss;
                tag1.UserId = userID;
                
                BookmarkTags bt1 = new BookmarkTags { Bookmarks = book1, Tags = tag1 };
                ent.BookmarkTags.Add(bt1);

            }
            ent.SaveChanges();
            ent.Dispose();
        }

         public bool registerUser(string username, string password)
        {
            Entities ent = new Entities();
            AspNetUsers newUser = new AspNetUsers();
            newUser.Email = username;
            newUser.PasswordHash = password;
            newUser.UserName = username;
            newUser.Id = Guid.NewGuid().ToString();
            ent.AspNetUsers.Add(newUser);
            try
            {
                ent.SaveChanges();
                ent.Dispose();
              
                return true;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                //throw raise;

                return false;
            }
            //ent.SaveChanges();
           
        }

       
    }
}
