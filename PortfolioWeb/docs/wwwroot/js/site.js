// Typing Animation
document.addEventListener('DOMContentLoaded', function () {
    // Text sequences
    const nameText = "Hello, I'm Ahmed";
    const dynamicTexts = [
        "A Software Engineer",
        "A Backend Developer",
        "A Problem Solver"
    ];
    const websiteText = "futuretech.com";

    // Type out name first
    typeText("#name-line", nameText, 100, () => {
        // Then type first dynamic text
        typeText("#dynamic-line", dynamicTexts[0], 100, () => {
            // Start cycling through dynamic texts
            let index = 1;
            setInterval(() => {
                deleteText("#dynamic-line", 50, () => {
                    typeText("#dynamic-line", dynamicTexts[index % dynamicTexts.length], 100);
                    index++;
                });
            }, 3000);

            // After first cycle, type website
            setTimeout(() => {
                typeText("#website-line", websiteText, 100);
            }, 1000);
        });
    });

    // Back to Top Button
    const backToTopBtn = document.querySelector('.back-to-top');
    window.addEventListener('scroll', () => {
        if (window.pageYOffset > 300) {
            backToTopBtn.classList.add('active');
        } else {
            backToTopBtn.classList.remove('active');
        }
    });

    backToTopBtn.addEventListener('click', (e) => {
        e.preventDefault();
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });

    // Add fade-in animation to sections when they come into view
    const sections = document.querySelectorAll('section');
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('fade-in');
            }
        });
    }, { threshold: 0.1 });

    sections.forEach(section => {
        observer.observe(section);
    });

    // Smooth scrolling for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            document.querySelector(this.getAttribute('href')).scrollIntoView({
                behavior: 'smooth'
            });
        });
    });
});

// Typing function
function typeText(element, text, speed, callback) {
    let i = 0;
    const target = document.querySelector(element);
    target.classList.add('typing');

    function typing() {
        if (i < text.length) {
            target.textContent = text.substring(0, i + 1);
            i++;
            setTimeout(typing, speed);
        } else {
            target.classList.remove('typing');
            if (callback) callback();
        }
    }

    typing();
}

// Deleting function
function deleteText(element, speed, callback) {
    const target = document.querySelector(element);
    let text = target.textContent;
    let i = text.length;
    target.classList.add('typing');

    function deleting() {
        if (i > 0) {
            target.textContent = text.substring(0, i - 1);
            i--;
            setTimeout(deleting, speed);
        } else {
            target.classList.remove('typing');
            if (callback) callback();
        }
    }

    deleting();
}