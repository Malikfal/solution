import { defineStore } from 'pinia';
import api from '@/services/api';

export const useTimeEntriesStore = defineStore('timeEntries', {
  state: () => ({
    entries: [],
    loading: false
  }),
  actions: {
    async fetchEntriesByDate(date) {
      this.loading = true;
      try {
        const res = await api.getTimeEntriesByDate(date);
        this.entries = res.data;
      } catch (error) {
        console.error(error);
      } finally {
        this.loading = false;
      }
    },
    async addEntry(entry) {
      const payload = {
        Date: entry.Date,
        Hours: entry.Hours,
        Description: entry.Description,
        TaskId: entry.TaskId
      };
      const res = await api.createTimeEntry(payload);
      this.entries.push(res.data);
    },
    async updateEntry(id, entry) {
      const payload = {
        Id: id,
        Date: entry.Date,
        Hours: entry.Hours,
        Description: entry.Description,
        TaskId: entry.TaskId
      };
      await api.updateTimeEntry(id, payload);
      const index = this.entries.findIndex(e => e.Id === id);
      if (index !== -1) this.entries[index] = { ...this.entries[index], ...payload };
    },
    async deleteEntry(id) {
      await api.deleteTimeEntry(id);
      this.entries = this.entries.filter(e => e.Id !== id);
    }
  }
});