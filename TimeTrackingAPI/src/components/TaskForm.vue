<template>
  <div class="modal-overlay" @click.self="$emit('close')">
    <div class="modal-card">
      <div class="modal-header">
        <h3>{{ isEditing ? '✏️ Редактировать задачу' : '➕ Новая задача' }}</h3>
        <button class="close-btn" @click="$emit('close')">✕</button>
      </div>

      <div class="modal-body">
        <!-- Название задачи -->
        <div class="form-group">
          <label>Название задачи</label>
          <input type="text" v-model="form.Name" placeholder="" autofocus />
        </div>

        <!-- Выбор проекта -->
        <div class="form-group">
          <label>Проект</label>
          <select v-model="form.ProjectId" class="modern-select">
            <option :value="null" disabled>— Выберите проект —</option>
            <option v-for="p in projects" :key="p.Id" :value="p.Id">
              {{ p.Name }} ({{ p.Code }})
            </option>
          </select>
        </div>

        <!-- Активность -->
        <div class="form-group checkbox">
          <label class="checkbox-label">
            <input type="checkbox" v-model="form.IsActive" />
            <span>Задача активна</span>
          </label>
        </div>
      </div>

      <div class="modal-footer">
        <button class="btn-cancel" @click="$emit('close')">Отмена</button>
        <button class="btn-save" @click="submit">Сохранить</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useTasksStore } from '@/stores/tasks';
import { useProjectsStore } from '@/stores/projects';

const props = defineProps({ task: Object, projectId: Number });
const emit = defineEmits(['close', 'saved']);

const tasksStore = useTasksStore();
const projectsStore = useProjectsStore();
const projects = ref([]);
const form = ref({
  Name: '',
  ProjectId: null,
  IsActive: true
});
const isEditing = ref(false);

onMounted(async () => {
  await projectsStore.fetchProjects();
  projects.value = projectsStore.projects;
  
  if (props.task) {
    isEditing.value = true;
    form.value = { ...props.task };
  } else if (props.projectId) {
    form.value.ProjectId = props.projectId;
  }
});

const submit = async () => {
  // Простая валидация
  if (!form.value.Name?.trim()) {
    alert('Введите название задачи');
    return;
  }
  if (!form.value.ProjectId) {
    alert('Выберите проект');
    return;
  }

  try {
    if (isEditing.value) {
      await tasksStore.updateTask(props.task.Id, form.value);
    } else {
      await tasksStore.addTask(form.value);
    }
    emit('saved');
    emit('close');
  } catch (err) {
    alert(err.response?.data || 'Ошибка сохранения');
  }
};
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.6);
  backdrop-filter: blur(4px);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
  animation: fadeIn 0.2s ease;
}


.modal-card {
  background: #ffffff;
  border-radius: 20px;
  width: 520px;
  max-width: 90%;
  box-shadow: 0 25px 45px rgba(0, 0, 0, 0.25);
  overflow: hidden;
  animation: slideUp 0.25s ease;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.2rem 1.5rem;
  background: linear-gradient(135deg, #f5f9ff 0%, #eef2f8 100%);
  border-bottom: 1px solid #e2e8f0;
}
.modal-header h3 {
  margin: 0;
  font-size: 1.35rem;
  font-weight: 600;
  color: #1e2a3a;
}
.close-btn {
  background: none;
  border: none;
  font-size: 1.5rem;
  cursor: pointer;
  color: #7f8c8d;
  transition: all 0.2s;
  line-height: 1;
  padding: 0 6px;
}
.close-btn:hover {
  color: #e53e3e;
  transform: scale(1.1);
}


.modal-body {
  padding: 1.5rem;
}
.form-group {
  margin-bottom: 1.3rem;
}
.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 500;
  color: #2d3748;
  font-size: 0.9rem;
}
.form-group input[type="text"],
.form-group select {
  width: 100%;
  padding: 0.7rem 1rem;
  border: 1px solid #cbd5e0;
  border-radius: 12px;
  font-size: 1rem;
  transition: all 0.2s;
  box-sizing: border-box;
  background: #fff;
}
.form-group input[type="text"]:focus,
.form-group select:focus {
  outline: none;
  border-color: #4a90e2;
  box-shadow: 0 0 0 3px rgba(74, 144, 226, 0.2);
}


.checkbox-label {
  display: flex;
  align-items: flex-start;
  gap: 0.5rem;
  cursor: pointer;
  flex-wrap: wrap;
}
.checkbox-label input {
  width: 18px;
  height: 18px;
  margin-top: 2px;
  cursor: pointer;
}
.checkbox-label span:first-of-type {
  font-weight: 500;
  color: #2d3748;
}

.modal-footer {
  padding: 1rem 1.5rem 1.5rem;
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  background: #fafcff;
}
.btn-cancel, .btn-save {
  padding: 0.6rem 1.2rem;
  border: none;
  border-radius: 40px;
  font-size: 0.9rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s;
}
.btn-cancel {
  background: #edf2f7;
  color: #4a5568;
}
.btn-cancel:hover {
  background: #e2e8f0;
  transform: translateY(-1px);
}
.btn-save {
  background: linear-gradient(105deg, #2c6e9e, #1e4a76);
  color: white;
  box-shadow: 0 2px 6px rgba(0,0,0,0.1);
}
.btn-save:hover {
  background: linear-gradient(105deg, #1e5a85, #163f60);
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(30, 90, 133, 0.3);
}


@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}
@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>