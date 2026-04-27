<template>
  <div>
    <h2>Учёт времени</h2>
    <div>
      <label>Выберите дату:</label>
      <input type="date" v-model="selectedDate" @change="loadEntries" />
    </div>
    <div class="sticker" :class="summary.stickerColor">
      {{ summary.message }} ({{ summary.totalHours }} ч.)
    </div>
    <button @click="showEntryForm = true">+ Добавить проводку</button>
    <ul>
      <li v-for="entry in entries" :key="entry.id">
        {{ entry.date }} – {{ entry.hours }} ч. - {{ entry.description }}
        (Задача: {{ entry.taskName }})
        <button @click="editEntry(entry)">✏️</button>
        <button @click="deleteEntry(entry.id)">❌</button>
      </li>
    </ul>
    <TimeEntryForm 
      v-if="showEntryForm" 
      :date="selectedDate"
      @close="showEntryForm = false" 
      @saved="loadEntries" 
    />
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue';
import api from '@/services/api';
import TimeEntryForm from './TimeEntryForm.vue';

const selectedDate = ref(new Date().toISOString().slice(0,10));
const entries = ref([]);
const summary = ref({ totalHours: 0, stickerColor: '', message: '' });
const showEntryForm = ref(false);

const loadEntries = async () => {
  const [entriesRes, summaryRes] = await Promise.all([
    api.getTimeEntriesByDate(selectedDate.value),
    api.getDailySummary(selectedDate.value)
  ]);
  entries.value = entriesRes.data;
  summary.value = summaryRes.data;
};
const deleteEntry = async (id) => {
  if (confirm('Удалить проводку?')) {
    await api.deleteTimeEntry(id);
    loadEntries();
  }
};
onMounted(loadEntries);
watch(selectedDate, loadEntries);
</script>

<style scoped>
.sticker { padding: 8px; border-radius: 8px; margin: 10px 0; }
.sticker.yellow { background-color: #fff3cd; color: #856404; }
.sticker.green { background-color: #d4edda; color: #155724; }
.sticker.red { background-color: #f8d7da; color: #721c24; }
</style>