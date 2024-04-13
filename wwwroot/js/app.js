

const loginNode = document.querySelector('#loginnode'),
      registerNode = document.querySelector('#registernode'),
      toRegisterLink = document.querySelector('.login__redirect'),
      toLoginLink = document.querySelector('.register__redirect');

      toRegisterLink.addEventListener('click', ()=>{
        loginNode.classList.add('none');
        registerNode.classList.remove('none');
      });

      toLoginLink.addEventListener('click', ()=>{
        loginNode.classList.remove('none');
        registerNode.classList.add('none');
      });