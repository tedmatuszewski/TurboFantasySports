<template>
    <div class="row py-4">
        <div class="col-md-7">
            <div class="row g-3">
                <div class="col-12">
                    <div class="d-flex align-items-start gap-3 p-3">
                        <span class="fs-3">🎉</span>
                        <div>
                            <h5 class="mb-1 fw-semibold">100% Free to Play</h5>
                            <p class="mb-0 text-muted">No subscriptions, no paywalls. Just pure fantasy racing fun!</p>
                        </div>
                    </div>
                </div>

                <!-- Draft Style -->
                <div class="col-12">
                    <div class="d-flex align-items-start gap-3 p-3">
                        <span class="fs-3">🎯</span>
                        <div>
                            <h5 class="mb-1 fw-semibold">True Draft-Style Leagues</h5>
                            <p class="mb-0 text-muted">The only platform offering authentic draft leagues for Supercross — pick your riders, build your strategy!</p>
                        </div>
                    </div>
                </div>

                <!-- League Management -->
                <div class="col-12">
                    <div class="d-flex align-items-start gap-3 p-3">
                        <span class="fs-3">👥</span>
                        <div>
                            <h5 class="mb-1 fw-semibold">Private League Creation</h5>
                            <p class="mb-0 text-muted">Create custom leagues, invite friends, and compete all season long!</p>
                        </div>
                    </div>
                </div>

                <!-- Weekly Management -->
                <div class="col-12">
                    <div class="d-flex align-items-start gap-3 p-3">
                        <span class="fs-3">🔄</span>
                        <div>
                            <h5 class="mb-1 fw-semibold">Weekly Team Management</h5>
                            <p class="mb-0 text-muted">Add or drop riders, set your lineup, and adapt your strategy each race!</p>
                        </div>
                    </div>
                </div>

                <!-- Mobile Friendly -->
                <div class="col-12">
                    <div class="d-flex align-items-start gap-3 p-3">
                        <span class="fs-3">📱</span>
                        <div>
                            <h5 class="mb-1 fw-semibold">Mobile-Friendly Design</h5>
                            <p class="mb-0 text-muted">Manage your team on the go — optimized for all devices!</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-5 d-flex align-items-center justify-content-center">
            <Donate />
        </div>
    </div>
</template>

<script setup>
  import Donate from "../components/Donate.vue";
</script>