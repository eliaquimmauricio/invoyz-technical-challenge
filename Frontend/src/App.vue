<template>
  <v-app>
    <v-app-bar color="primary" prominent>
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
      
      <v-toolbar-title>
        <v-icon class="mr-2">mdi-file-document-outline</v-icon>
        Invoyz
      </v-toolbar-title>

      <v-spacer></v-spacer>

      <v-btn icon @click="toggleTheme">
        <v-icon>{{ theme.global.name.value === 'light' ? 'mdi-weather-night' : 'mdi-weather-sunny' }}</v-icon>
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
  theme.global.name.value = theme.global.name.value === 'light' ? 'dark' : 'light'
}
</script>

<style>
html {
  overflow-y: auto;
}
</style>
