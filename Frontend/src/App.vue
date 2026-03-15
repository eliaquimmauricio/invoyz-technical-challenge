<template>
  <v-app>
    <v-app-bar color="surface" elevation="0" class="app-bar">
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
      
      <v-toolbar-title class="d-flex align-center">
        <span class="brand-title">Invoyz</span>
      </v-toolbar-title>

      <v-spacer></v-spacer>

      <v-btn icon @click="toggleTheme" variant="text">
        <v-icon>{{ theme.global.name.value === 'invoyzLight' ? 'mdi-weather-night' : 'mdi-weather-sunny' }}</v-icon>
      </v-btn>
    </v-app-bar>

    <v-navigation-drawer v-model="drawer" temporary>
      <v-list>
        <v-list-item
          prepend-icon="mdi-view-dashboard"
          title="Dashboard"
          to="/"
        ></v-list-item>
        
        <v-list-item
          prepend-icon="mdi-account-multiple"
          title="Customers"
          to="/customers"
        ></v-list-item>

        <v-list-item
          prepend-icon="mdi-package-variant"
          title="Products"
          to="/products"
        ></v-list-item>

        <v-list-item
          prepend-icon="mdi-file-document"
          title="Invoices"
          to="/invoices"
        ></v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-main>
      <router-view></router-view>
    </v-main>

    <v-snackbar
      v-model="snackbar.show"
      :color="snackbar.color"
      :timeout="3000"
      location="top right"
    >
      {{ snackbar.text }}
      <template v-slot:actions>
        <v-btn variant="text" @click="snackbar.show = false">Close</v-btn>
      </template>
    </v-snackbar>
  </v-app>
</template>

<script setup>
import { ref } from 'vue'
import { useTheme } from 'vuetify'
import { useNotificationStore } from './stores/notification'
import { storeToRefs } from 'pinia'

const theme = useTheme()
const drawer = ref(false)

const notificationStore = useNotificationStore()
const { snackbar } = storeToRefs(notificationStore)

const toggleTheme = () => {
  theme.global.name.value = theme.global.name.value === 'invoyzLight' ? 'invoyzDark' : 'invoyzLight'
}
</script>

<style>
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700;800;900&display=swap');

html {
  overflow-y: auto;
}

* {
  font-family: 'Inter', sans-serif !important;
}

.brand-title {
  font-size: 24px;
  font-weight: 700;
  letter-spacing: -0.02em;
  background: linear-gradient(135deg, #6366F1 0%, #8B5CF6 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.app-bar {
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.v-card {
  border-radius: 12px !important;
}

.v-btn {
  text-transform: none !important;
  font-weight: 600 !important;
  letter-spacing: normal !important;
}

.text-h3, .text-h4, .text-h5, .text-h6 {
  font-weight: 700 !important;
  letter-spacing: -0.02em !important;
}
</style>
