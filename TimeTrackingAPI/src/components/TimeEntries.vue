<template>
  <div>
    <h2>Учёт времени</h2>
    <div class="controls">
      <div class="date-picker">
        <label>Дата:</label>
        <input type="date" v-model="selectedDate" @change="loadEntries" />
      </div>
      <button @click="showEntryForm = true" class="add-btn">+ Добавить проводку</button>
    </div>

      <div class="sticker" :class="summary.stickerColor">
        {{ summary.message }} ({{ summary.totalHours }} ч.)
      </div>

    <div v-if="entriesStore.loading">Загрузка...</div>
    <ul v-else class="entries-list">
      <li v-for="entry in entriesStore.entries" :key="entry.Id" class="entry-item">
        <div class="entry-info">
          <span class="entry-date">{{ formatDate(entry.Date) }}</span>
          <span class="entry-hours">{{ entry.Hours }} ч.</span>
          <span class="entry-description">{{ entry.Description || '—' }}</span>
        </div>
        <div class="action-buttons">
          <button @click="editEntry(entry)" class="edit-btn">✏️</button>
          <button @click="deleteEntry(entry.Id)" class="delete-btn">❌</button>
        </div>
      </li>
    </ul>

    <TimeEntryForm 
      v-if="showEntryForm || editingEntry" 
      :date="selectedDate"
      :entry="editingEntry"
      @close="closeForm" 
      @saved="loadEntries" 
    />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useTimeEntriesStore } from '@/stores/timeEntries';
import api from '@/services/api';
import TimeEntryForm from './TimeEntryForm.vue';

const entriesStore = useTimeEntriesStore();
const selectedDate = ref(new Date().toISOString().slice(0,10));
const summary = ref({ totalHours: 0, stickerColor: '', message: '' });
const showEntryForm = ref(false);
const editingEntry = ref(null);

const formatDate = (dateStr) => dateStr?.split('T')[0] || '';

const loadEntries = async () => {
  await entriesStore.fetchEntriesByDate(selectedDate.value);
  const res = await api.getDailySummary(selectedDate.value);
  // 👇 Адаптация под заглавные буквы бэкенда
  summary.value = {
    totalHours: res.data.TotalHours ?? 0,
    stickerColor: res.data.StickerColor ?? 'yellow',
    message: res.data.Message ?? 'Нет данных'
  };
};

const deleteEntry = async (id) => {
  if (confirm('Удалить проводку?')) {
    await entriesStore.deleteEntry(id);
    await loadEntries();
  }
};

const editEntry = (entry) => {
  editingEntry.value = entry;
  showEntryForm.value = true;
};

const closeForm = () => {
  showEntryForm.value = false;
  editingEntry.value = null;
};

onMounted(loadEntries);
</script>

<style scoped>
.controls {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 1rem;
  flex-wrap: wrap;
}
.date-picker {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}
.add-btn {
  background-color: #4caf50;
  color: white;
  border: none;
  padding: 0.3rem 0.8rem;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.9rem;
}
.add-btn:hover {
  background-color: #45a049;
}
.sticker {
  padding: 0.5rem 1rem;
  border-radius: 8px;
  margin: 0.5rem 0 1rem 0;
  font-weight: 500;
  display: inline-block;
}
.sticker.yellow {
  background-color: #fff3cd;
  color: #856404;
}
.sticker.green {
  background-color: #d4edda;
  color: #155724;
}
.sticker.red {
  background-color: #f8d7da;
  color: #721c24;
}
.entries-list {
  list-style: none;
  padding: 0;
  margin: 1rem 0;
}
.entry-item {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0.5rem 0;
  border-bottom: 1px solid #eee;
  flex-wrap: wrap;
  gap: 0.5rem;
}
.entry-info {
  display: flex;
  align-items: baseline;
  gap: 1rem;
  flex-wrap: wrap;
}
.entry-date {
  font-weight: 600;
  min-width: 100px;
}
.entry-hours {
  background-color: #e9ecef;
  padding: 0.2rem 0.5rem;
  border-radius: 16px;
  font-size: 0.85rem;
  font-weight: 500;
}
.entry-description {
  color: #555;
  font-size: 0.9rem;
}
.action-buttons {
  display: flex;
  gap: 0.5rem;
}
button {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 1.2rem;
  padding: 0.2rem;
  transition: opacity 0.2s;
}
button:hover {
  opacity: 0.7;
}
</style>