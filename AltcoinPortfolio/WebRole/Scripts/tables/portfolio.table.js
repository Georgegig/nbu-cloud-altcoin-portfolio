'use strict';

let PortfolioTable = (function(){
        
    let deleteUserPortfolio = () => {
        let portfolioTable = JSON.parse(localStorage.getItem('portfolioTable')) ? 
            JSON.parse(localStorage.getItem('portfolioTable')): [];
        let user = JSON.parse(localStorage.getItem('user'));
        let userPortfolioIndex = portfolioTable ? _.findIndex(portfolioTable, (el) => {
            return el.user == user.email;
        }) : -1;
        if(userPortfolioIndex != -1){
            portfolioTable[userPortfolioIndex].portfolio = [];
            localStorage.setItem('portfolioTable', JSON.stringify(portfolioTable));
        }
    };

    return {
        deleteUserPortfolio: deleteUserPortfolio
    };
})();