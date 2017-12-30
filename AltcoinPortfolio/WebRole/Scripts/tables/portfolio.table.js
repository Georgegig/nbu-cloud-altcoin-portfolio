'use strict';

let PortfolioTable = (function(){
    let updateUserPortfolio = (newPortfolio) => {
        let portfolioTable = JSON.parse(localStorage.getItem('portfolioTable')) ? 
            JSON.parse(localStorage.getItem('portfolioTable')): [];
        let user = JSON.parse(localStorage.getItem('user'));
        let userPortfolioIndex = portfolioTable ? _.findIndex(portfolioTable, (el) => {
            return el.user == user.email;
        }) : -1;
        if(userPortfolioIndex != -1){
            portfolioTable[userPortfolioIndex].portfolio = newPortfolio;
        }
        else {
            portfolioTable.push({
                user: user.email,
                portfolio: newPortfolio
            });
        }
        localStorage.setItem('portfolioTable', JSON.stringify(portfolioTable));
    };
    
    let getUserPortfolio = () => {           
        let portfolioTable = JSON.parse(localStorage.getItem('portfolioTable'));
        let user = JSON.parse(localStorage.getItem('user'));
        let userPortfolioIndex = portfolioTable ? _.findIndex(portfolioTable, (el) => {
            return el.user == user.email;
        }) : -1;
        if(userPortfolioIndex != -1){
            return portfolioTable[userPortfolioIndex].portfolio;
        }
        else {
            return null;
        }
    };
    
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
        updateUserPortfolio: updateUserPortfolio,
        getUserPortfolio: getUserPortfolio,
        deleteUserPortfolio: deleteUserPortfolio
    };
})();