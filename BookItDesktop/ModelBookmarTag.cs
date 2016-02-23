using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookItDesktop
{
    public class ModelBookmarTag
    {
        public Bookmarks bookmarkedPage
        {
            get; set;
        }

        public List<Tags> tagList
        {
            get; set;
        }

    }
}
