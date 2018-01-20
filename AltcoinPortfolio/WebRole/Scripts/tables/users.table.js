'use strict';

let UsersTable = (function () {     
    
    let userLoggedIn = () => {
        var user = JSON.parse(localStorage.getItem('user'));
        if (user) {
            if (user.timeStamp &&
                ((new Date(user.timeStamp)).addDays(1)).getTime() > (new Date()).getTime()) {
                return true;
            }
            else {
                localStorage.removeItem('user');
                return false;
            }
        }
        return false;
    };
  
    let loginUser = (email) => {
        let loginDate = new Date();
        let user = {
            email: email,
            timeStamp: loginDate.getFullYear() + '-' + (loginDate.getMonth() + 1) + '-' + loginDate.getDate()
        };
        localStorage.setItem('user', JSON.stringify(user));
    };

    let getLoggedUserMail = () => {
        return JSON.parse(localStorage.getItem('user')).email;
    };

    let logoutUser = () => {
        localStorage.removeItem('user');
    };
    
    return {
        userLoggedIn: userLoggedIn,
        loginUser: loginUser,
        getLoggedUserMail: getLoggedUserMail,
        logoutUser: logoutUser
    }
})();