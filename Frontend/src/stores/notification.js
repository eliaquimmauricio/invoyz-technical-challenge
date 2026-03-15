import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useNotificationStore = defineStore('notification', () => {
  const snackbar = ref({
    show: false,
    text: '',
    color: 'success'
  })

  function showSuccess(text) {
    snackbar.value = {
      show: true,
      text,
      color: 'success'
    }
  }

  function showError(text) {
    snackbar.value = {
      show: true,
      text,
      color: 'error'
    }
  }

  function showInfo(text) {
    snackbar.value = {
      show: true,
      text,
      color: 'info'
    }
  }

  return {
    snackbar,
    showSuccess,
    showError,
    showInfo
  }
})
