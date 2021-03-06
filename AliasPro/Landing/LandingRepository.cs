﻿using AliasPro.API.Landing.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Landing
{
    internal class LandingRepository
    {
        private readonly LandingDao _landingDao;
        private IList<IHallOfFamer> _hallOfFamers;
        private IList<IArticle> _articles;

        public LandingRepository(LandingDao dao)
        {
            _landingDao = dao;
        }

        internal async Task<IList<IHallOfFamer>> GetHallOfFamersAsync()
        {
            if (_hallOfFamers != null) return _hallOfFamers;

            _hallOfFamers = await _landingDao.GetHallOfFamers();
            return _hallOfFamers;
        }

        internal async Task<IList<IArticle>> GetNewsArticlesAsync()
        {
            if (_articles != null) return _articles;

            _articles = await _landingDao.GetNewsArticles();
            return _articles;
        }
    }
}
