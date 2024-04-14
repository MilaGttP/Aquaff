
      /**profile logik */

      const openNotification = document.querySelector('.profile__notification');
      const notificationNode = document.querySelector('.notification');
      const closeNotificationModal = document.querySelector('.close__notification');
      openNotification.addEventListener('click',()=>{
        notificationNode.classList.add('open__notification')
      });

      closeNotificationModal.addEventListener('click',()=>{
        notificationNode.classList.remove('open__notification');
      });