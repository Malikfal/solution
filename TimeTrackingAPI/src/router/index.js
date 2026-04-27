import { createRouter, createWebHistory } from 'vue-router';
import ProjectsList from '@/components/ProjectsList.vue';
import TasksList from '@/components/TasksList.vue';
import TimeEntries from '@/components/TimeEntries.vue';

const routes = [
  { path: '/', redirect: '/projects' },
  { path: '/projects', component: ProjectsList },
  { path: '/tasks', component: TasksList },
  { path: '/time', component: TimeEntries }
];

const router = createRouter({ history: createWebHistory(), routes });
export default router;